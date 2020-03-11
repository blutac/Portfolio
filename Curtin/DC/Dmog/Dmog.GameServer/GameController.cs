/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Timers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dmog.ServerSupport;
using Dmog.GameObjects;

namespace Dmog.GameServer
{
    /// <summary>
    /// Triggered On end of round
    /// </summary>
    /// <param name="gameLog">The log of the round</param>
    public delegate void EndRoundEventHandler(GameEventLog gameLog);

    /// <summary>
    /// Tiggered On end of game
    /// </summary>
    /// <param name="win">The win result of the game</param>
    public delegate void EndGameEventHandler(bool win);

    /// <summary>
    /// Controls game logic
    /// </summary>
    public class GameController
    {
        ///// FIELDS /////////////////////////////////////
        private int MaxPlayers;
        private int MinPlayers;
        private int RoundPeriod;
        private Timer RoundTimer;
        private Random Rand;
        
        private int RoundCount;         // Stores the current round iteration
        private GameEventLog GameLog;   // Stores all game events for broadcasting to other players
        
        private Boss GameBoss;          // Stores the current boss of the game

        /// <summary>
        /// Catched BossTable
        /// </summary>
        public List<Boss> BossTable { get; set; }
        /// <summary>
        /// Catched HeroTable
        /// </summary>
        public List<Hero> HeroTable { get; set; }

        private ClientSessionRegistry PlayerRegistry;
        private EndRoundEventHandler EndRoundHandler;
        private EndGameEventHandler EndGameHandler;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public GameController(int roundPeriod, int minPlayers, int maxPlayers,
                              List<Boss> bossTable, List<Hero> heroTable, ClientSessionRegistry playerRegistry,
                              EndRoundEventHandler endRoundHandler, EndGameEventHandler endGameHandler)
        {
            if (maxPlayers < minPlayers)
                throw new ArgumentException("max player limit cannot be less than min player limit");
            
            MaxPlayers = maxPlayers;
            MinPlayers = minPlayers;
            RoundPeriod = roundPeriod;
            IntialiseRoundTimer(RoundPeriod);

            RoundCount = 0;
            GameLog = new GameEventLog();
            Rand = new Random();
            
            GameBoss = null;
            BossTable = bossTable;
            HeroTable = heroTable;
            PlayerRegistry = playerRegistry;
            EndRoundHandler = endRoundHandler;
            EndGameHandler = endGameHandler;
        }
        //////////////////////////////////////////////////

        #region "///// TIMER METHODS /////"
        private void IntialiseRoundTimer(int interval)
        {
            RoundTimer = new Timer();
            RoundTimer.Interval = interval;
            RoundTimer.Elapsed += new ElapsedEventHandler(OnNextRound);
            RoundTimer.Enabled = true;
            RoundTimer.AutoReset = true;
            RoundTimer.Stop();
        }

        private void DistroyRoundTimer()
        {
            if (RoundTimer != null)
            {
                RoundTimer.Stop();
                RoundTimer.Close();
                RoundTimer = null;
            }
        }

        private bool StartRoundTimer()
        {
            bool success = false;
            Console.Write("> Starting Round Timer... ");

            try
            {
                RoundTimer.Start();
                success = true;
                Console.WriteLine("OK!");
            } catch (NullReferenceException) {
                Console.WriteLine("ERROR!");
            } catch (ArgumentOutOfRangeException) {
                Console.WriteLine("ERROR!");
            }
            
            return success;
        }

        private bool StopRoundTimer()
        {
            bool success = false;
            Console.Write("> Stopping Round Timer... ");

            try
            {
                RoundTimer.Stop();
                success = true;
                Console.WriteLine("OK!");
            } catch (NullReferenceException) {
                Console.WriteLine("ERROR!");
            }
            
            return success;
        }
        #endregion


        #region "///// EVENT LOG METHODS /////"
        private void LogEvent(string actor, string[] targets,
                              AbilityEffect abilityEffect, int effectMagnitude)
        {
            GameEvent gameEvent = new GameEvent(RoundCount, actor, targets,
                                                abilityEffect, effectMagnitude);
            LogEvent(gameEvent);
        }

        private void LogEvent(GameEvent gameEvent)
        {
            if (gameEvent != null)
            {
                GameLog.GameEventList.Add(gameEvent);
                PrintGameEvent(gameEvent);
            }
            else
                Console.WriteLine("> [GAME EVENT] WARNING: null game event generated!");
        }

        private void PrintGameEvent(GameEvent gameEvent)
        {
            Console.Write("> [GAME EVENT] player:{" + gameEvent.Actor + "}"
                        + " dealt:{" + gameEvent.AbilityEffect.ToString() + "}"
                        + " of value:{" + gameEvent.EffectMagnitude + "}"
                        + " at: ");

            foreach (string target in gameEvent.Targets)
            {
                Console.Write("{" + target + "} ");
            }
            Console.WriteLine();
        }

        private GameEventLog GetGameEvents(int round)
        {
            GameEventLog log = new GameEventLog();
            foreach (GameEvent ge in GameLog.GameEventList)
            {
                if (ge.Round == round)
                    log.GameEventList.Add(ge);
            }

            return log;
        }
        #endregion


        #region "///// GAME STATES /////"
        /// <summary>
        /// returns true if the game is waiting for a precondition to be met to enter play state
        /// </summary>
        public bool IsGameInPendingState()
        {
            return IsGameLoopRunning() && HasGameStarted() == false;
        }

        /// <summary>
        /// returns true if the game has started and boss and players are alive
        /// </summary>
        public bool IsGameInPlayState()
        {
            return IsGameLoopRunning()
                && (HasGameStarted() == true)
                && (IsBossAlive() == true && IsPlayersAlive() == true);
        }

        /// <summary>
        /// returns true if the game has started and boss or all players are dead
        /// </summary>
        public bool IsGameInEndState()
        {
            return IsGameLoopRunning()
                && (HasGameStarted() == true)
                && (IsBossAlive() == false || IsPlayersAlive() == false);
        }

        /// <summary>
        /// returns true if the min player condition has been met and boss is alive
        /// </summary>
        public bool IsGameAbleToEnterPlayState()
        {
            return (IsGameLoopRunning() && IsEnoughValidPlayers() && IsBossAlive());
        }

        public bool ArePlayersAllowedToEnterGame()
        {
            return (IsGameLoopRunning() && IsEnoughPlayers() && IsBossAlive());
        }

        public bool IsGameLoopRunning()
        {
            return (RoundTimer.Enabled);
        }

        public bool IsEnoughValidPlayers()
        {
            int validPlayers = 0;
            foreach (ClientSession cs in PlayerRegistry.GetClientSessionArray())
            {
                if (((PlayerSession)cs).PlayerHero != null)
                {
                    validPlayers++;
                }
            }

            return (validPlayers >= MinPlayers);
        }

        public bool IsEnoughPlayers()
        {
            return (PlayerRegistry.GetCount() >= MinPlayers);
        }

        public bool HasGameStarted()
        {
            return (RoundCount > 0);
        }

        public bool IsBossAlive()
        {
            return (GameBoss != null) && (GameBoss.HpCurrent > 0);
        }

        public bool IsPlayersAlive()
        {
            foreach (ClientSession cs in PlayerRegistry.GetClientSessionArray())
            {
                if (((PlayerSession)cs).IsAlive())
                    return true;
            }
            return false;
        }
        #endregion


        #region "///// GAME METHODS /////"
        /// <summary>
        /// The game round loop
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void OnNextRound(object source, ElapsedEventArgs e)
        {
            if (IsGameInPlayState() || IsGameAbleToEnterPlayState())
            {
                RoundCount++;
                Console.WriteLine("> [GAME EVENT] ROUND (" + RoundCount + ")");

                // Play all user turns if able //
                foreach (ClientSession cs in PlayerRegistry.GetClientSessionArray())
                {
                    PlayerSession player = (PlayerSession)cs;
                    if (player.IsPlayable())
                    {
                        PlayUserTurn(player);
                        player.Move = null; // clear move
                    }
                }

                // Play Boss //
                PlayServerTurn(GameBoss);

                // Signal End Round //
                EndRoundHandler(GetGameEvents(RoundCount)); // send this round's game log

                if (IsGameInEndState())
                {
                    Console.WriteLine("> [GAME EVENT] Game has entered end state!");
                    StopGame();
                    bool win = false;
                    
                    if (IsBossAlive() == false)
                    {
                        Console.WriteLine("> [GAME EVENT] Boss is Dead!");
                        win = true;
                    }

                    if (IsPlayersAlive() == false)
                    {
                        Console.WriteLine("> [GAME EVENT] All Players Dead!");
                        win = false;
                    }

                    // Clear Player Heroes //
                    foreach (ClientSession cs in PlayerRegistry.GetClientSessionArray())
                    {
                        ((PlayerSession)cs).PlayerHero = null;
                    }

                    // Signal End Game //
                    EndGameHandler(win);
                }
            }
            else
            {
                Console.WriteLine("> [GAME EVENT] Game is in pending state!");
            }

            // Enable any waiting players //
            foreach (ClientSession cs in PlayerRegistry.GetClientSessionArray())
            {
                if (((PlayerSession)cs).PlayerHero != null)
                    ((PlayerSession)cs).Waiting = false;
            }
        }
        
        public bool StartNewGame()
        {
            Console.WriteLine("> [GAME EVENT] Starting New Game");

            // Reset game state //
            GameBoss = null;
            RoundCount = 0;
            GameLog = new GameEventLog();

            bool success = (
                SelectRandomBoss()
                &&
                StartRoundTimer() // only executes if previous statement returns true
            );

            return success;
        }

        public void StopGame()
        {
            StopRoundTimer();
            Console.WriteLine("> [GAME EVENT] Game Stopped");
        }

        public BotListing GetBotListing()
        {
            BotListing botList = new BotListing();
            botList.BotDetailList.Add(new BotDetail(Boss.PUID, GameBoss));
            return botList;
        }
        #endregion



        #region "///// GAME TURN METHODS /////"
        private void PlayServerTurn(Boss boss)
        {
            try
            {
                // Unpack boss move //
                int abilitySelection = 0; // boss only has 1 ability
                string targetSelection = null;
                Ability ability = boss.Abilities[abilitySelection];

                // Calculate & Perform move //
                int effectMagnitude = GenerateBossEffectMagnitude(ability.Value);
                List<string> targets = GetBossTargets(targetSelection, ability.TargetStrategy, ability.TargetType);
                ApplyEffect(targets, ability.AbilityEffect, effectMagnitude);

                LogEvent(Boss.PUID, targets.ToArray(), ability.AbilityEffect, effectMagnitude); // Log this event
            } catch (NullReferenceException) {
                Console.WriteLine("> [GAME EVENT] WARNING: server turn failed!");
            } catch (IndexOutOfRangeException) {
                Console.WriteLine("> [GAME EVENT] WARNING: server turn failed!");
            }
        }

        private void PlayUserTurn(PlayerSession player)
        {
            try
            {
                // Unpack player move //
                int abilitySelection = player.Move.SelectedAbilityId;
                string targetSelection = player.Move.SelectedTarget;
                Ability ability = player.PlayerHero.Abilities[abilitySelection];

                // Calculate & Perform move //
                int effectMagnitude = GenerateHeroEffectMagnitude(ability.Value);
                List<string> targets = GetHeroTargets(targetSelection, ability.TargetStrategy, ability.TargetType);
                ApplyEffect(targets, ability.AbilityEffect, effectMagnitude);

                LogEvent(player.PUID, targets.ToArray(), ability.AbilityEffect, effectMagnitude); // Log this event
            } catch (NullReferenceException) {
                Console.WriteLine("> [GAME EVENT] WARNING: player turn failed!");
            } catch (IndexOutOfRangeException) {
                Console.WriteLine("> [GAME EVENT] WARNING: player turn failed!");
            }
        }

        /// <summary>
        /// Gets a list of targets for a Hero,
        /// given the initially chosen target and target strategy & target type
        /// </summary>
        /// <param name="target">a username (or the Boss.PUID)</param>
        /// <returns>A list of usernames (or the Boss.PUID)</returns>
        private List<string> GetHeroTargets(string target, TargetStrategy ts, TargetType tt)
        {
            List<string> targets = new List<string>();

            if (target == Boss.PUID)
                targets.Add(target);
            else
            {
                switch (tt)
                {
                    case TargetType.Single:
                        targets.Add(target);
                        break;
                    case TargetType.Team:
                        targets.AddRange(GetPlayerTeam(Hero.TEAM));
                        break;
                }
            }

            return targets;
        }

        /// <summary>
        /// Gets a list of targets for a Boss,
        /// given the initially chosen target and target strategy & target type
        /// </summary>
        /// <param name="target">a username</param>
        /// <returns>A list of usernames</returns>
        private List<string> GetBossTargets(string target, TargetStrategy ts, TargetType tt)
        {
            List<string> targets = new List<string>();

            switch (ts)
            {
                case TargetStrategy.Manual:
                    targets.Add(target);
                    break;
                case TargetStrategy.HighestHitting:
                    targets.Add(GetHighestHittingPlayer());
                    break;
                case TargetStrategy.Random:
                    targets.Add(GetRandomPlayer());
                    break;
            }

            return targets;
        }

        /// <summary>
        /// Gets a list of players belonging to the specified team
        /// </summary>
        /// <returns>A list of usernames</returns>
        private List<string> GetPlayerTeam(string team)
        {
            List<string> players = new List<string>();
            foreach (ClientSession cs in PlayerRegistry.GetClientSessionArray())
            {
                PlayerSession player = (PlayerSession)cs;
                if (player.IsTargetable() && player.PlayerHero.Team == team)
                {
                    players.Add(player.PUID);
                }
            }

            return players;
        }
        
        private string GetHighestHittingPlayer()
        {
            string puid = null;
            int hitRecord = 0;

            foreach(ClientSession cs in PlayerRegistry.GetClientSessionArray())
            {
                PlayerSession player = (PlayerSession)cs;
                if (player.IsTargetable() && player.HitTally >= hitRecord)
                {
                    hitRecord = player.HitTally;
                    puid = player.PUID;
                }
            }

            return puid;
        }

        private string GetRandomPlayer()
        {
            string puid = null;

            // Compile selection list //
            List<PlayerSession> selection = new List<PlayerSession>();
            foreach (ClientSession cs in PlayerRegistry.GetClientSessionArray())
            {
                if (((PlayerSession)cs).IsTargetable())
                {
                    selection.Add((PlayerSession)cs);
                }
            }

            // Select Player //
            if (selection.Count > 0)
            {
                int index = Rand.Next(0, selection.Count);
                puid = selection[index].PUID;
            }
            
            return puid;
        }
        
        private int GenerateHeroEffectMagnitude(int value)
        {
            return Rand.Next(value / 4, value + 1);
        }

        private int GenerateBossEffectMagnitude(int value)
        {
            return Rand.Next(value / 2, value + 1);
        }

        private void ApplyEffect(List<string> targets, AbilityEffect ae, int magnitude)
        {
            PlayerSession player = null;

            foreach (string target in targets)
            {
                player = (PlayerSession)PlayerRegistry.GetClientSession(PlayerRegistry.GetToken(target));
                if (player != null && player.IsTargetable())
                {
                    ApplyEffect(player.PlayerHero, ae, magnitude);
                }
                else if (target == Boss.PUID)
                {
                    ApplyEffect(GameBoss, ae, magnitude);
                }
            }
        }
        
        private void ApplyEffect(Entity entity, AbilityEffect ae, int magnitude)
        {
            switch (ae)
            {
                case AbilityEffect.Damage:
                    ApplyHit(entity, magnitude);
                    break;
                case AbilityEffect.Heal:
                    ApplyHeal(entity, magnitude);
                    break;
            }
        }

        private void ApplyHit(Entity entity, int magnitude)
        {
            magnitude -= entity.Defence;
            if (magnitude < 0) magnitude = 0;

            int hp = entity.HpCurrent - magnitude;
            if (hp < 0) hp = 0;

            entity.HpCurrent = hp;
        }

        private void ApplyHeal(Entity entity, int magnitude)
        {
            int hp = entity.HpCurrent + magnitude;
            if (hp > entity.HpMax) hp = entity.HpMax;

            entity.HpCurrent = hp;
        }

        private bool SelectRandomBoss()
        {
            bool success = false;

            try
            {
                Console.Write("> [GAME EVENT] Selecting new boss... ");
                int index = Rand.Next(0, BossTable.Count);
                GameBoss = new Boss(BossTable[index]);
                success = true;
                Console.WriteLine("OK! Selected boss{" + GameBoss.Name + "}");

            } catch (ArgumentOutOfRangeException) {
                Console.WriteLine("ERROR! BossTable empty!");
            } catch (NullReferenceException) {
                Console.WriteLine("ERROR! Object is null!");
            }

            return success;
        }
        #endregion
    }
}

/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Dmog.ServerSupport;
using Dmog.GameObjects;
using Dmog.DataServer;
using Dmog.PortalServer;
using System.Runtime.CompilerServices;

namespace Dmog.GameServer
{
    /// <summary>
    /// The GameServer server object
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    internal class GameServerController : IGameServerController,
        IGamePortalServerController_Callback, IDataServerController_Callback
    {
        ///// FIELDS /////////////////////////////////////
        private const int MIN_PLAYERS = 5;
        private const int MAX_PLAYERS = 12;
        private const int ROUND_PERIOD = 30000;
        private const double PLAYER_LIFESPAN = 60;
        private const double GAME_LIFESPAN = 60 * 24;

        private string Token; // The game server token (issued by portal server)
        private DataServerConnection DSConnection;
        private GamePortalServerConnection GPSConnection;
        private ClientSessionRegistry PlayerRegistry; // Set of players connected to server
        private GameController Game;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public GameServerController()
        {
            PlayerRegistry = new ClientSessionRegistry();
            Console.WriteLine("Server Object initiated!");
        }

        ~GameServerController()
        {
            Console.WriteLine("Server Object terminated!");
        }
        //////////////////////////////////////////////////
        
        
        #region "///// SETUP METHODS /////"
        internal void InitiateDataServerConnection(string address)
        {
            try
            {
                Console.Write("> Opening Connection to Server: DmogDataServer... ");
                DSConnection = new DataServerConnection();
                DSConnection.InitiateConnection(this, address);
                Console.WriteLine("OK!");
            } catch (ServerConnectionException ex) {
                Console.WriteLine("ERROR!");
                throw ex;
            }
        }

        internal void InitiatePortalServerConnection(string address)
        {
            try
            {
                Console.Write("> Opening Connection to Server: DmogPortalServer... ");
                GPSConnection = new GamePortalServerConnection();
                GPSConnection.InitiateConnection(this, address);
                Console.WriteLine("OK!");
            } catch (ServerConnectionException ex) {
                Console.WriteLine("ERROR!");
                throw ex;
            }
        }

        internal void CloseDataServerConnection()
        {
            try
            {
                Console.Write("> Closing Connection to Server: DmogDataServer... ");
                if (DSConnection != null)
                {
                    DSConnection.CloseConnection();
                    DSConnection = null;
                }
                Console.WriteLine("OK!");
            } catch (ServerConnectionException ex) {
                Console.WriteLine("ERROR!");
                throw ex;
            }
        }

        internal void ClosePortalServerConnection()
        {
            try
            {
                Console.Write("> Closing Connection to Server: DmogPortalServer... ");
                if (GPSConnection != null)
                {
                    GPSConnection.CloseConnection();
                    GPSConnection = null;
                }
                Console.WriteLine("OK!");
            } catch (ServerConnectionException ex) {
                Console.WriteLine("ERROR!");
                throw ex;
            }
        }
        
        /// <summary>
        /// Registers this game server with the portal server
        /// </summary>
        /// <param name="address">game server address</param>
        /// <param name="serverName">game server name</param>
        internal void RegisterServer(string address, string serverName)
        {
            try
            {
                Console.Write("> Registering with PortalServer... ");
                Token = GPSConnection.RegisterGameServer(address, serverName);
                Console.WriteLine("OK!");

            } catch (ServerConnectionException ex) {
                Console.WriteLine("ERROR!");
                throw ex;
            } catch (PortalServerException ex) {
                Console.WriteLine("ERROR!");
                throw ex;
            }
        }

        /// <summary>
        /// Unregisters this server with the portal server
        /// </summary>
        internal void UnregisterServer()
        {
            try
            {
                Console.Write("> Unregistering with PortalServer... ");
                GPSConnection.UnregisterGameServer(Token);
                Console.WriteLine("OK!");

            } catch (ServerConnectionException ex) {
                Console.WriteLine("ERROR!");
                throw ex;
            } catch (PortalServerException ex) {
                Console.WriteLine("ERROR!");
                throw ex;
            }
        }
        #endregion


        #region "///// PLAYER CALLBACK TRIGGERS /////"
        internal void Trigger_OnPlayerChange(PlayerDetail playerDetail, string userToken)
        {
            PlayerSession session = (PlayerSession)PlayerRegistry.GetClientSession(userToken);
            if (session != null)
            {
                IGameServerController_Callback callback = session.CallbackChannel;
                if (callback != null)
                {
                    try
                    {
                        callback.GameServer_OnPlayerChange(playerDetail);
                    } catch (CommunicationException ex) {
                        Console.WriteLine("> Failed trigger event: OnPlayerChange\n" + ex.Message);
                    } catch (TimeoutException ex) {
                        Console.WriteLine("> Failed trigger event: OnPlayerChange\n" + ex.Message);
                    }
                }
                else Console.WriteLine("> Failed trigger event: OnPlayerChange\n" + "callback object null!");
            }
            else Console.WriteLine("> Failed trigger event: OnPlayerChange\n" + "PlayerSession not found!");
        }
        
        internal void Trigger_OnPlayerLeave(string userPuid, string userToken)
        {
            PlayerSession session = (PlayerSession)PlayerRegistry.GetClientSession(userToken);
            if (session != null)
            {
                IGameServerController_Callback callback = session.CallbackChannel;
                if (callback != null)
                {
                    try
                    {
                        callback.GameServer_OnPlayerLeave(userPuid);
                    } catch (CommunicationException ex) {
                        Console.WriteLine("> Failed trigger event: OnPlayerLeave\n" + ex.Message);
                    } catch (TimeoutException ex) {
                        Console.WriteLine("> Failed trigger event: OnPlayerLeave\n" + ex.Message);
                    }
                }
                else Console.WriteLine("> Failed trigger event: OnPlayerLeave\n" + "callback object null!");
            }
            else Console.WriteLine("> Failed trigger event: OnPlayerLeave\n" + "PlayerSession not found!");
        }
        
        internal void Trigger_OnGameReady(bool ready, string userToken)
        {
            PlayerSession session = (PlayerSession)PlayerRegistry.GetClientSession(userToken);
            if (session != null)
            {
                IGameServerController_Callback callback = session.CallbackChannel;
                if (callback != null)
                {
                    try
                    {
                        callback.GameServer_OnGameReady(ready);
                    } catch (CommunicationException ex) {
                        Console.WriteLine("> Failed trigger event: OnGameReady\n" + ex.Message);
                    } catch (TimeoutException ex) {
                        Console.WriteLine("> Failed trigger event: OnGameReady\n" + ex.Message);
                    }
                }
                else Console.WriteLine("> Failed trigger event: OnGameReady\n" + "callback object null!");
            }
            else Console.WriteLine("> Failed trigger event: OnGameReady\n" + "PlayerSession not found!");
        }
        
        internal void Trigger_OnGameEnd(bool win, string userToken)
        {
            PlayerSession session = (PlayerSession)PlayerRegistry.GetClientSession(userToken);
            if (session != null)
            {
                IGameServerController_Callback callback = session.CallbackChannel;
                if (callback != null)
                {
                    try
                    {
                        callback.GameServer_OnGameEnd(win);
                    } catch (CommunicationException ex) {
                        Console.WriteLine("> Failed trigger event: OnGameEnd\n" + ex.Message);
                    } catch (TimeoutException ex) {
                        Console.WriteLine("> Failed trigger event: OnGameEnd\n" + ex.Message);
                    }
                }
                else Console.WriteLine("> Failed trigger event: OnGameEnd\n" + "callback object null!");
            }
            else Console.WriteLine("> Failed trigger event: OnGameEnd\n" + "PlayerSession not found!");
        }
        
        internal void Trigger_OnRoundEnd(BotListing botListing, PlayerListing playerListing, GameEventLog gameLog, string userToken)
        {
            PlayerSession session = (PlayerSession)PlayerRegistry.GetClientSession(userToken);
            if (session != null)
            {
                IGameServerController_Callback callback = session.CallbackChannel;
                if (callback != null)
                {
                    try
                    {
                        callback.GameServer_OnRoundEnd(botListing, playerListing, gameLog);
                    } catch (CommunicationException ex) {
                        Console.WriteLine("> Failed trigger event (CommunicationException): OnRoundEnd\n" + ex.Message);
                    } catch (TimeoutException ex) {
                        Console.WriteLine("> Failed trigger event (TimeoutException): OnRoundEnd\n" + ex.Message);
                    }
                }
                else Console.WriteLine("> Failed trigger event: OnRoundEnd\n" + "callback object null!");
            }
            else Console.WriteLine("> Failed trigger event: OnRoundEnd\n" + "PlayerSession not found!");
        }
        
        internal void Trigger_OnPing(string userToken)
        {
            PlayerSession session = (PlayerSession)PlayerRegistry.GetClientSession(userToken);
            if (session != null)
            {
                IGameServerController_Callback callback = session.CallbackChannel;
                if (callback != null)
                {
                    try
                    {
                        callback.GameServer_OnPing();
                    } catch (CommunicationException ex) {
                        Console.WriteLine("> Failed trigger event: OnPing\n" + ex.Message);
                    } catch (TimeoutException ex) {
                        Console.WriteLine("> Failed trigger event: OnPing\n" + ex.Message);
                    }
                }
                else Console.WriteLine("> Failed trigger event: OnPing\n" + "callback object null!");
            }
            else Console.WriteLine("> Failed trigger event: OnPing\n" + "PlayerSession not found!");
        }
        internal void Trigger_OnPing()
        {
            foreach (KeyValuePair<string, ClientSession> kvp in PlayerRegistry)
            {
                Trigger_OnPing(kvp.Key);
            }
        }
        #endregion

        
        #region "///// CLIENT BROADCAST METHODS /////"
        internal void AdvertiseToPortal_UserJoin(string userPuid)
        {
            try
            {
                Console.Write("> Advertising to PortalServer: @username:{" + userPuid + "} joined game... ");
                GPSConnection.AddUserToPlayerList(Token, userPuid);
                Console.WriteLine("OK!");

            } catch (ServerConnectionException) {
                Console.WriteLine("ERROR! Lost connection to PortalServer");
            }
        }

        internal void AdvertiseToPortal_UserLeave(string userPuid)
        {
            try
            {
                Console.Write("> Advertising to PortalServer: @username:{" + userPuid + "} left game... ");
                GPSConnection.RemoveUserFromPlayerList(Token, userPuid);
                Console.WriteLine("OK!");

            } catch (ServerConnectionException) {
                Console.WriteLine("ERROR! Lost connection to PortalServer");
            }
        }
        
        internal void BroadcastToPlayers_PlayerStatus(string userToken)
        {
            Console.WriteLine("> Broadcasting to Players: @userToken:{" + userToken + "} status change");
            PlayerDetail pd = null;
            PlayerSession ps = (PlayerSession)PlayerRegistry.GetClientSession(userToken);
            if (ps != null) pd = ps.ExportToPlayerDetail();

            foreach (KeyValuePair<string, ClientSession> kvp in PlayerRegistry)
            {
                Trigger_OnPlayerChange(pd, kvp.Key);
            }
        }
        
        internal void BroadcastToPlayers_PlayerLeave(string userPuid)
        {
            Console.WriteLine("> Broadcasting to Players: @username:{" + userPuid + "} left game");
            foreach (KeyValuePair<string, ClientSession> kvp in PlayerRegistry)
            {
                Trigger_OnPlayerLeave(userPuid, kvp.Key);
            }
        }
        
        internal void BroadcastToPlayers_GameReady(bool ready)
        {
            Console.WriteLine("> Broadcasting to Players: GameReady:{" + ready + "}");
            foreach (KeyValuePair<string, ClientSession> kvp in PlayerRegistry)
            {
                Trigger_OnGameReady(ready, kvp.Key);
            }
        }
        
        internal void BroadcastToPlayers_GameEnd(bool win)
        {
            Console.WriteLine("> Broadcasting to Players: GameEnd:{Win=" + win + "}");
            foreach (KeyValuePair<string, ClientSession> kvp in PlayerRegistry)
            {
                Trigger_OnGameEnd(win, kvp.Key);
            }
        }
        
        internal void BroadcastToPlayers_RoundEnd(BotListing botListing, PlayerListing playerListing, GameEventLog gameLog)
        {
            Console.WriteLine("> Broadcasting to Players: RoundEnd");
            foreach (KeyValuePair<string, ClientSession> kvp in PlayerRegistry)
            {
                Trigger_OnRoundEnd(botListing, playerListing, gameLog, kvp.Key);
            }
        }
        #endregion


        #region "///// GAME METHODS /////"
        public PlayerListing GetPlayerListing()
        {
            PlayerListing plist = new PlayerListing();

            foreach (KeyValuePair<string, ClientSession> kvp in PlayerRegistry)
            {
                plist.PlayerDetailList.Add(((PlayerSession)kvp.Value).ExportToPlayerDetail());
            }

            return plist;
        }

        public BotListing GetBotListing()
        {
            BotListing botlist = null;
            if (Game != null)
                botlist = Game.GetBotListing();
            
            return botlist;
        }

        /// <summary>
        /// Gets Boss table from Data server
        /// </summary>
        internal List<Boss> GetBossTable()
        {
            List<Boss> bosses = null;

            try
            {
                Console.Write("> Getting Boss table from DataServer... ");
                bosses = new List<Boss>(DSConnection.GetBossTable());
                Console.WriteLine("OK!");
            } catch (DataServerException ex) {
                Console.WriteLine("ERROR!");
                throw ex;
            }

            return bosses;
        }

        /// <summary>
        /// Gets Hero table from Data server
        /// </summary>
        internal List<Hero> GetHeroTable()
        {
            List<Hero> heroes = null;

            try
            {
                Console.Write("> Getting Hero table from DataServer... ");
                heroes = new List<Hero>(DSConnection.GetHeroTable());
                Console.WriteLine("OK!");
            } catch (DataServerException ex) {
                Console.WriteLine("ERROR!");
                throw ex;
            }

            return heroes;
        }

        public void InitialiseGame()
        {
            try
            {
                List<Boss> bosses = GetBossTable();
                List<Hero> heroes = GetHeroTable();
                Game = new GameController(ROUND_PERIOD, MIN_PLAYERS, MAX_PLAYERS,
                                            bosses, heroes, PlayerRegistry, Game_OnEndRound, Game_OnEndGame);
            } catch (DataServerException ex) {
                throw ex;
            } catch (ArgumentException ex) {
                throw ex;
            }
        }
        
        public void Game_OnEndRound(GameEventLog gameLog)
        {
            Console.WriteLine("> End Round Event Triggered");
            BroadcastToPlayers_RoundEnd(GetBotListing(), GetPlayerListing(), gameLog);
        }
        
        public void Game_OnEndGame(bool win)
        {
            Console.WriteLine("> End Game Event Triggered: Win state = " + win);
            BroadcastToPlayers_GameEnd(win);
            Game.StartNewGame();
        }

        public void StartNewGame()
        {
            Game.StartNewGame();
        }

        public void StopGame()
        {
            Game.StopGame();
        }
        #endregion


        #region "///// USER METHODS /////"
        public void Reconnect(string userToken)
        {
            if (PlayerRegistry.IsClientRegistered(userToken))
            {
                IGameServerController_Callback callbackChannel
                    = OperationContext.Current.GetCallbackChannel<IGameServerController_Callback>();
                ((PlayerSession)PlayerRegistry.GetClientSession(userToken)).CallbackChannel = callbackChannel; // update callback channel
                Console.WriteLine("> Player Reconnect: @token:{" + userToken + "}");
            }
        }

        public bool JoinGameServer(string userPuid, string userToken, out ECode ec)
        {
            ec = ECode.None;
            bool success = false;
            IGameServerController_Callback callbackChannel // Get player callback object to store
                = OperationContext.Current.GetCallbackChannel<IGameServerController_Callback>();
            
            try
            {
                if (PlayerRegistry.GetCount() >= MAX_PLAYERS)
                {
                    ec = ECode.ServerFull;
                    Console.WriteLine("> Rejected Player Join: @username:{" + userPuid + "} token:{" + userToken + "}");
                }
                else if (PlayerRegistry.IsClientRegistered(userToken))
                {
                    ((PlayerSession)PlayerRegistry.GetClientSession(userToken)).CallbackChannel = callbackChannel; // update callback channel
                    success = true;
                    Console.WriteLine("> Old Player Join: @username:{" + userPuid + "} token:{" + userToken + "}");
                }
                else if (GPSConnection.IsUserTokenVaild(userToken))
                {
                    // Register Player //
                    PlayerSession ps = new PlayerSession(userPuid, "", PLAYER_LIFESPAN, callbackChannel, null);
                    PlayerRegistry.RegisterClient(userToken, ps);
                    success = true;
                    Console.WriteLine("> New Player Join: @username:{" + userPuid + "} token:{" + userToken + "}");

                    // Notify others of event //
                    AdvertiseToPortal_UserJoin(userPuid);
                    //BroadcastToPlayers_PlayerStatus(userPuid);
                    if (Game != null && Game.HasGameStarted() == false)
                    {   // Notify players if this will change game state
                        BroadcastToPlayers_GameReady(Game.ArePlayersAllowedToEnterGame());
                    }
                }
                else Console.WriteLine("> Invalid Player Join: @username:{" + userPuid + "} token:{" + userToken + "}");

            } catch (ServerConnectionException ex) {
                ec = ex.ErrorCode;
                Console.WriteLine("> Failed Player Join: @username:{" + userPuid + "} token:{" + userToken + "}");
            }
            
            return success;
        }
        
        public void LeaveGameServer(string userPuid, string userToken, out ECode ec)
        {
            ec = ECode.None;

            if (PlayerRegistry.IsClientRegistered(userToken, userPuid) == true)
            {
                // Unregister Player //
                PlayerRegistry.UnregisterClient(userToken);
                Console.WriteLine("> New user leave request: @username:{" + userPuid + "}");

                // Notify others of event //
                AdvertiseToPortal_UserLeave(userPuid);
                //BroadcastToPlayers_PlayerLeave(userPuid);
                if (Game != null && Game.HasGameStarted() == false)
                {   // Notify players if this will change game state
                    BroadcastToPlayers_GameReady(Game.ArePlayersAllowedToEnterGame());
                }
            }
            else
            {
                Console.WriteLine("> Invalid user leave request: @username:{" + userPuid + "}");
                ec = ECode.QueryError;
            }
        }

        public void SubmitPlayerHero(string userToken, int heroId, out ECode ec)
        {
            ec = ECode.None;

            if (PlayerRegistry.IsClientRegistered(userToken))
            {
                PlayerSession ps = (PlayerSession)PlayerRegistry.GetClientSession(userToken);
                try
                {
                    ps.PlayerHero = DSConnection.GetHero(heroId); // Change to get Hero from local catched copy
                } catch (NullReferenceException) {
                    ec = ECode.QueryError;
                } catch (DataServerException) {
                    ec = ECode.QueryError;
                }
            }
            else
                ec = ECode.AuthenticationFail;
        }

        public void SubmitPlayerMove(string userToken, PlayerMove move, out ECode ec)
        {
            ec = ECode.None;

            if (PlayerRegistry.IsClientRegistered(userToken))
            {
                PlayerSession ps = (PlayerSession)PlayerRegistry.GetClientSession(userToken);
                try
                {
                    ps.Move = move;
                } catch (NullReferenceException) {
                    ec = ECode.QueryError;
                }
            }
            else
                ec = ECode.AuthenticationFail;
        }

        public bool QueryGameReadyState(string userToken, out ECode ec)
        {
            ec = ECode.None;
            return (Game != null) && (Game.IsGameInPlayState() || Game.ArePlayersAllowedToEnterGame());
        }

        public PlayerListing QueryPlayerListing(string userToken, out ECode ec)
        {
            ec = ECode.None;
            PlayerListing playerListing = null;

            if (PlayerRegistry.IsClientRegistered(userToken))
                playerListing = GetPlayerListing();
            else
                ec = ECode.AuthenticationFail;
            
            return playerListing;
        }
        #endregion


        bool IDataServerController_Callback.DataServer_OnPing() { return true; }
        bool IGamePortalServerController_Callback.GamePortalServer_OnPing() { return true; }
    }
}

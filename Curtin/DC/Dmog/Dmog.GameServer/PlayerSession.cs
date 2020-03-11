/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using Dmog.ServerSupport;
using Dmog.GameObjects;

namespace Dmog.GameServer
{
    /// <summary>
    /// Represents a Game Server client connection session
    /// </summary>
    public class PlayerSession : ClientSession
    {
        ///// FIELDS /////////////////////////////////////
        public IGameServerController_Callback CallbackChannel { get; set; }
        public Hero PlayerHero { get; set; }
        public PlayerMove Move { get; set; }

        /// <summary>
        /// Stores a tally of the hits this player made against the boss
        /// </summary>
        public int HitTally { get; set; } // should probably be stored server side to avoid resetting by rejoining
        /// <summary>
        /// Indicates if the player is in play during a game round
        /// </summary>
        public bool Waiting { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public PlayerSession(string username, string address, double lifeSpan,
            IGameServerController_Callback callbackChannel, Hero userHero)
            : base(username, address, lifeSpan)
        {
            CallbackChannel = callbackChannel;
            PlayerHero = userHero;
            Move = null;
            HitTally = 0;
            Waiting = true;
        }
        //////////////////////////////////////////////////

        /// <summary>
        /// Returns true if the player is alive
        /// </summary>
        public bool IsAlive()
        {
            return (Waiting == true) || (PlayerHero != null && PlayerHero.HpCurrent > 0);
        }
        
        /// <summary>
        /// Returns true if the player can be targeted by other players
        /// </summary>
        public bool IsTargetable()
        {
            return (IsAlive() && Waiting == false);
        }

        /// <summary>
        /// Returns true if the player can perform a move
        /// </summary>
        public bool IsPlayable()
        {
            return (IsAlive() && Waiting == false && Move != null);
        }
        
        /// <summary>
        /// Returns a copy of the player state for update/information purposes
        /// </summary>
        public PlayerDetail ExportToPlayerDetail()
        {
            return new PlayerDetail(this.PUID, this.PlayerHero);
        }
        
    }
}

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

namespace Dmog.PortalServer
{
    /// <summary>
    /// Represents a Game Server client connection session
    /// </summary>
    public class GameSession : ClientSession
    {
        ///// FIELDS /////////////////////////////////////
        /// <summary>
        /// Stores the portal side copy of the Game Server's player list.
        /// Stored as a Dictionary of [PUID (username), session-token] pairs
        /// </summary>
        public Dictionary<string, string> PlayerList { get; set; } // Set of session tokens connected to server
        public IGamePortalServerController_Callback CallbackChannel { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public GameSession(string gameServerName, string gameServerAddress, double lifeSpan,
            IGamePortalServerController_Callback callbackChannel)
            : base(gameServerName, gameServerAddress, lifeSpan)
        {
            if (callbackChannel == null)
                throw new ArgumentNullException("callbackChannel cannot be null");

            CallbackChannel = callbackChannel;
            PlayerList = new Dictionary<string, string>();
        }
        //////////////////////////////////////////////////

        public bool AddUserToPlayerList(string userPuid, string userToken)
        {
            bool success = false;
            try
            {
                if (userPuid != null && userToken != null)
                {
                    PlayerList.Add(userPuid, userToken);
                    success = true;
                }
            } catch (ArgumentException) {}

            return success;
        }

        public bool RemoveUserFromPlayerList(string userPuid)
        {
            bool success = false;
            if (userPuid != null)
                success = PlayerList.Remove(userPuid);

            return success;
        }
    }

    /// <summary>
    /// Represents a User client connection session
    /// </summary>
    public class UserSession : ClientSession
    {
        ///// FIELDS /////////////////////////////////////
        public User UserData { get; set; }
        public IUserPortalServerController_Callback CallbackChannel { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public UserSession(string username, string address, double lifeSpan,
            IUserPortalServerController_Callback callbackChannel, User userData)
            : base(username, address, lifeSpan)
        {
            if (callbackChannel == null)
                throw new ArgumentNullException("callbackChannel cannot be null");
    
            if (userData == null)
                throw new ArgumentNullException("userData cannot be null");

            CallbackChannel = callbackChannel;
            UserData = userData;
        }
        //////////////////////////////////////////////////
    }
}

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

namespace Dmog.PortalServer
{
    /// <summary>
    /// The PortalServer server object
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    internal class PortalServerController : IPortalServerController,
        IDataServerController_Callback
    {
        ///// FIELDS /////////////////////////////////////
        // Client Session lifespans //
        private const double USER_LIFESPAN = 60;
        private const double GAME_LIFESPAN = 60 * 24;

        private DataServerConnection DSConnection;
        private ClientSessionRegistry GameRegistry;
        private ClientSessionRegistry UserRegistry;
        //////////////////////////////////////////////////
        
        ///// STRUCTORS //////////////////////////////////
        public PortalServerController()
        {
            GameRegistry = new ClientSessionRegistry();
            UserRegistry = new ClientSessionRegistry();
            Console.WriteLine("Server Object initiated!");
        }

        ~PortalServerController()
        {
            Console.WriteLine("Server Object terminated!");
        }
        //////////////////////////////////////////////////
        
        #region "///// INTERNAL METHODS /////"
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

        internal GameListing GenerateGameServerListing()
        {
            GameListing gsl = null;
            List<GameDetail> list = new List<GameDetail>();

            foreach (KeyValuePair<string, ClientSession> kvp in GameRegistry)
            {
                string name = kvp.Value.PUID;
                string address = kvp.Value.ClientAddress;
                List<string> registeredUsernames
                    = new List<string>(((GameSession)kvp.Value).PlayerList.Keys);
                
                list.Add(new GameDetail(name, address, registeredUsernames));
            }

            gsl = new GameListing(list);
            return gsl;
        }
        #endregion
        

        #region "///// GAME CONTROLLER METHODS /////"
        public string RegisterGameServer(string address, string serverName, out ECode ec)
        {
            ec = ECode.None;
            string token = null;
            bool collision = false;

            try
            {
                // Check for GameServer collisions //
                collision = (GameRegistry.IsPuidRegistered(serverName)); // Check if already registered
                if (collision == false)
                {
                    // Check for server address collisions
                    foreach (KeyValuePair<string, ClientSession> kvp in GameRegistry)
                    {
                        if (collision == false) // If no collision detected, check for collision
                            collision = (kvp.Value.ClientAddress == address);
                    }
                }

                if (collision)
                {
                    ec = ECode.QueryError;
                    Console.WriteLine("> GameServer registration collision: @ServerName:{" + serverName + "}");
                }
                else
                {
                    IGamePortalServerController_Callback callbackChannel
                            = OperationContext.Current.GetCallbackChannel<IGamePortalServerController_Callback>();
                    GameSession session = new GameSession(serverName, address, GAME_LIFESPAN, callbackChannel);
                    token = GameRegistry.RegisterClient(session);
                    Console.WriteLine("> New GameServer registration: @ServerName:{" + serverName + "} token:{" + token + "}");
                }

            } catch (ArgumentNullException) {
                // occurs if serverName or address is null
                ec = ECode.QueryError;
            }

            return token;
        }

        public void UnregisterGameServer(string token, out ECode ec)
        {
            ec = ECode.None;
            GameRegistry.UnregisterClient(token);
            Console.WriteLine("> Requested GameServer unregistration: @token:{" + token + "}");
        }
        
        public void AddUserToPlayerList(string token, string userPuid)
        {
            GameSession gs = (GameSession)GameRegistry.GetClientSession(token);

            if (gs != null)
            {
                string userToken = UserRegistry.GetToken(userPuid);
                if (userToken != null)
                {
                    gs.AddUserToPlayerList(userPuid, userToken);
                    Console.WriteLine("> Event: @username:{" + userPuid + "} joined @GameServer:{" + gs.PUID + "}");
                }
                else
                {
                    Console.WriteLine("> Illegal Event: @username:{" + userPuid + "} joined @GameServer:{" + gs.PUID + "}");
                    Console.WriteLine("> Reason: @username:{" + userPuid + "} has invaild session token:{" + userToken + "}");
                }
            }
            else
                Console.WriteLine("> Illegal Event: @username:{" + userPuid + "} joined unknown GameServer!");
        }

        public void RemoveUserFromPlayerList(string token, string userPuid)
        {
            GameSession gs = (GameSession)GameRegistry.GetClientSession(token);

            if (gs != null)
            {
                gs.RemoveUserFromPlayerList(userPuid);
                Console.WriteLine("> Event: @username{" + userPuid + "} left @GameServer:{" + gs.PUID + "}");
            }
            else
                Console.WriteLine("> Illegal Event: @username{" + userPuid + "} left unknown GameServer!");
        }

        public bool IsUserTokenVaild(string userToken)
        {
            return UserRegistry.IsClientRegistered(userToken);
        }

        public Boss[] GetBossTable(string token, out ECode ec)
        {
            ec = ECode.None;
            Boss[] bosses = null;

            if (UserRegistry.IsTokenRegistered(token))
            {
                try
                {
                    bosses = DSConnection.GetBossTable();
                } catch (ServerConnectionException) {
                    ec = ECode.ConnectionError;
                } catch (DataServerException) {
                    ec = ECode.QueryError;
                }
            }
            else
                ec = ECode.AuthenticationFail;

            return bosses;
        }
        #endregion
        

        #region "///// USER CONTROLLER METHODS /////"
        /* returns null if:
                - already logged in
                - database connection failed
                - database authentication failed
                - user data is null
                - username is null
        */
        public string Login(string address, string username, string password, out ECode ec)
        {
            ec = ECode.None;
            string token = null;
            IUserPortalServerController_Callback callbackChannel
                = OperationContext.Current.GetCallbackChannel<IUserPortalServerController_Callback>();

            try
            {
                // Check credentials against database //
                int id = DSConnection.AuthenticateCredentials(username, password);
                if (id != -1) // valid
                {
                    // Check if already logged in //
                    if (UserRegistry.IsPuidRegistered(username) == false)
                    {
                        // Log in User //
                        User user = DSConnection.GetUser(id);
                        UserSession session = new UserSession(username, address, USER_LIFESPAN, callbackChannel, user);
                        token = UserRegistry.RegisterClient(session);
                        Console.WriteLine("> New Login: @username:{" + username + "} token:{" + token + "}");
                    }
                    else
                    {   // Retrieve existing session token //
                        token = UserRegistry.GetToken(username);
                        ((UserSession)UserRegistry.GetClientSession(token)).CallbackChannel = callbackChannel; // update callback channel
                        Console.WriteLine("> Old Login: @username:{" + username + "} token:{" + token + "}");
                    }
                }
                else
                {
                    Console.WriteLine("> Failed Login: @username:{" + username + "}");
                }

            } catch (DataServerException ex) {
                // occurs if any of the database queries fail
                ec = ex.ErrorCode;
            } catch (ArgumentNullException) {
                // occurs if username or user data is null
                ec = ECode.QueryError;
            }
            return token;
        }
        
        public void Logout(string token, out ECode ec)
        {
            ec = ECode.None;
            UserRegistry.UnregisterClient(token);
            Console.WriteLine("> Requested Logout: @token:{" + token + "}");
        }
        
        public bool PingGameServer(string serverName)
        {
            bool result = false;
            string gameToken = GameRegistry.GetToken(serverName);
            GameSession gs = (GameSession)GameRegistry.GetClientSession(gameToken);

            if (gs != null)
            {
                try
                {
                    // The ping test fails if any kind of Exception is thrown
                    result = gs.CallbackChannel.GamePortalServer_OnPing();
                } catch (Exception) {
                    // Ping failed!
                }
            }

            return result;
        }
        
        public GameListing QueryGameServers(string token, out ECode ec)
        {
            ec = ECode.None;
            GameListing gsl = null;

            if (UserRegistry.IsClientRegistered(token))
                gsl = GenerateGameServerListing();
            else
                ec = ECode.AuthenticationFail;

            return gsl;
        }
        
        public string[] QueryFriends(string token, out ECode ec)
        {
            ec = ECode.None;
            string[] result = null;

            try
            {
                UserSession session = (UserSession)UserRegistry.GetClientSession(token);
                result = session.UserData.Friends;
            } catch (NullReferenceException) {
                ec = ECode.QueryError;
            }

            return result;
        }

        public string[] QueryFriendsOnline(string token, out ECode ec)
        {
            ec = ECode.None;
            string[] result = null;
            string[] friends = QueryFriends(token, out ec);

            if (friends != null)
            {
                List<string> matches = new List<string>();
                foreach (string username in friends)
                {
                    if (UserRegistry.IsPuidRegistered(username))
                        matches.Add(username);
                }
                result = matches.ToArray();
            }

            return result;
        }
        
        public Hero[] GetHeroTable(string token, out ECode ec)
        {
            ec = ECode.None;
            Hero[] heroes = null;

            if (UserRegistry.IsTokenRegistered(token))
            {
                try
                {
                    heroes = DSConnection.GetHeroTable();
                } catch (ServerConnectionException) {
                    ec = ECode.ConnectionError;
                } catch (DataServerException) {
                    ec = ECode.QueryError;
                }
            }
            else
                ec = ECode.AuthenticationFail;

            return heroes;
        }
        #endregion
        
        
        bool IDataServerController_Callback.DataServer_OnPing() { return true; }
    }
}

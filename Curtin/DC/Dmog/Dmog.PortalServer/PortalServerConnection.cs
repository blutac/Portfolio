/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.ServiceModel;
using Dmog.GameObjects;
using Dmog.ServerSupport;

namespace Dmog.PortalServer
{
    /// <summary>
    /// Provides the Game-client side connection implementation to the Portal Server.
    /// </summary>
    public class GamePortalServerConnection : ServerConnection<IGamePortalServerController, IGamePortalServerController_Callback>
    {
        ///// FIELDS /////////////////////////////////////
        protected const string ERRMSG_CONNECTION = "Failed to connect to portal server!";
        //////////////////////////////////////////////////
        
        public string RegisterGameServer(string address, string serverName)
        {
            ECode ec = ECode.None;
            string value = null;

            try
            {
                value = Server.RegisterGameServer(address, serverName, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new PortalServerException(ec, "Internal error occurred at: RegisterGameServer()");

            return value;
        }
        
        public void UnregisterGameServer(string token)
        {
            ECode ec = ECode.None;

            try
            {
                Server.UnregisterGameServer(token, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new PortalServerException(ec, "Internal error occurred at: DeregisterGameServer()");
        }
        
        public void AddUserToPlayerList(string token, string userPuid)
        {
            try
            {
                Server.AddUserToPlayerList(token, userPuid);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }
        }
        
        public void RemoveUserFromPlayerList(string token, string userPuid)
        {
            try
            {
                Server.RemoveUserFromPlayerList(token, userPuid);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }
        }
        
        public bool IsUserTokenVaild(string userToken)
        {
            bool value = false;

            try
            {
                value = Server.IsUserTokenVaild(userToken);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }
            
            return value;
        }
        
        public Boss[] GetBossTable(string token)
        {
            ECode ec = ECode.None;
            Boss[] value = null;

            try
            {
                value = Server.GetBossTable(token, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new PortalServerException(ec, "Internal error occurred at: GetBossTable()");

            return value;
        }
    }

    /// <summary>
    /// Provides the User-client side connection implementation to the Data Server.
    /// </summary>
    public class UserPortalServerConnection : ServerConnection<IUserPortalServerController, IUserPortalServerController_Callback>
    {
        ///// FIELDS /////////////////////////////////////
        protected const string ERRMSG_CONNECTION = "Failed to connect to portal server!";
        //////////////////////////////////////////////////
        
        public string Login(string address, string username, string password)
        {
            ECode ec = ECode.None;
            string value = null;

            try
            {
                value = Server.Login(address, username, password, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new PortalServerException(ec, "Internal error occurred at: Login()");

            return value;
        }
        
        public void Logout(string token)
        {
            ECode ec = ECode.None;

            try
            {
                Server.Logout(token, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new PortalServerException(ec, "Internal error occurred at: Logout()");
        }
        
        public bool PingGameServer(string serverName)
        {
            bool value = false;
            try
            {
                value = Server.PingGameServer(serverName);
            } catch(Exception) {}

            return value;
        }
        
        public GameListing QueryGameServers(string token)
        {
            ECode ec = ECode.None;
            GameListing value = null;

            try
            {
                value = Server.QueryGameServers(token, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new PortalServerException(ec, "Internal error occurred at: QueryGameServers()");
            
            return value;
        }
        
        public string[] QueryFriends(string token)
        {
            ECode ec = ECode.None;
            string[] value = null;

            try
            {
                value = Server.QueryFriends(token, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new PortalServerException(ec, "Internal error occurred at: QueryFriends()");

            return value;
        }
        
        public string[] QueryFriendsOnline(string token)
        {
            ECode ec = ECode.None;
            string[] value = null;

            try
            {
                value = Server.QueryFriendsOnline(token, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new PortalServerException(ec, "Internal error occurred at: QueryFriendsOnline()");

            return value;
        }
        
        public Hero[] GetHeroTable(string token)
        {
            ECode ec = ECode.None;
            Hero[] value = null;

            try
            {
                value = Server.GetHeroTable(token, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new PortalServerException(ec, "Internal error occurred at: GetHeroTable()");

            return value;
        }
    }
}

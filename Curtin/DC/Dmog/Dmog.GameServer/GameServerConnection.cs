/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.ServiceModel;
using Dmog.ServerSupport;
using Dmog.GameObjects;
using System.Runtime.CompilerServices;

namespace Dmog.GameServer
{
    /// <summary>
    /// Provides the client side connection implementation to the Game Server.
    /// </summary>
    public class GameServerConnection : ServerConnection<IGameServerController, IGameServerController_Callback>
    {
        ///// FIELDS /////////////////////////////////////
        protected const string ERRMSG_CONNECTION = "Failed to connect to game server!";
        //////////////////////////////////////////////////
        
        public void Reconnect(string userToken)
        {
            try
            {
                Reconnect();
                Server.Reconnect(userToken);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }
        }

        public void Reconnect(IGameServerController_Callback callbackObject, string userToken)
        {
            try
            {
                Reconnect(callbackObject);
                Server.Reconnect(userToken);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }
        }
        
        public bool JoinGameServer(string userPuid, string userToken)
        {
            ECode ec = ECode.None;
            bool value = false;

            try
            {
                value = Server.JoinGameServer(userPuid, userToken, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new GameServerException(ec, "Internal error occurred at: JoinGameServer()");
            
            return value;
        }
        
        public void LeaveGameServer(string userPuid, string userToken)
        {
            ECode ec = ECode.None;

            try
            {
                Server.LeaveGameServer(userPuid, userToken, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new GameServerException(ec, "Internal error occurred at: LeaveGameServer()");
        }
        
        public void SubmitPlayerHero(string userToken, int heroId)
        {
            ECode ec = ECode.None;

            try
            {
                Server.SubmitPlayerHero(userToken, heroId, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new GameServerException(ec, "Internal error occurred at: SubmitPlayerHero()");
        }
        
        public void SubmitPlayerMove(string userToken, PlayerMove move)
        {
            ECode ec = ECode.None;

            try
            {
                Server.SubmitPlayerMove(userToken, move, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new GameServerException(ec, "Internal error occurred at: SubmitPlayerMove()");
        }
        
        public bool QueryGameReadyState(string userToken)
        {
            ECode ec = ECode.None;
            bool value = false;

            try
            {
                value = Server.QueryGameReadyState(userToken, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new GameServerException(ec, "Internal error occurred at: QueryGameReadyState()");
            
            return value;
        }
        
        public PlayerListing QueryPlayerListing(string userToken)
        {
            ECode ec = ECode.None;
            PlayerListing value = null;

            try
            {
                value = Server.QueryPlayerListing(userToken, out ec);
            } catch(ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, null, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new GameServerException(ec, "Internal error occurred at: GetPlayerListing()");
            
            return value;
        }
    }
}

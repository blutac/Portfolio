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

namespace Dmog.DataServer
{
    /// <summary>
    /// Provides the client side connection implementation to the Data Server.
    /// </summary>
    public class DataServerConnection : ServerConnection<IDataServerController, IDataServerController_Callback>
    {
        ///// FIELDS /////////////////////////////////////
        // Error Messages //
        private const string ERRMSG_CONNECTION = "Failed to connect to dataserver!";
        private const string ERRMSG_QUERY = "The dataserver rejected the query: ";
        //////////////////////////////////////////////////
        
        #region "///// METADATA GETTERS /////"
        public int GetUserCount()
        {
            ECode ec = ECode.None;
            int count = -1;

            try
            {
                count = Server.GetUserCount(out ec);
            } catch(ArgumentNullException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new DataServerException(ec, Address, ERRMSG_QUERY + "GetUserCount");

            return count;
        }

        public int GetHeroCount()
        {
            ECode ec = ECode.None;
            int count = -1;

            try
            {
                count = Server.GetHeroCount(out ec);
            } catch(ArgumentNullException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new DataServerException(ec, Address, ERRMSG_QUERY + "GetHeroCount");

            return count;
        }

        public int GetBossCount()
        {
            ECode ec = ECode.None;
            int count = -1;

            try
            {
                count = Server.GetBossCount(out ec);
            } catch(ArgumentNullException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new DataServerException(ec, Address, ERRMSG_QUERY + "GetBossCount");

            return count;
        }
        #endregion

        #region "///// GAME DATA GETTERS /////"
        public int AuthenticateCredentials(string username, string password)
        {
            ECode ec = ECode.None;
            int value = 0;

            try
            {
                value = Server.AuthenticateCredentials(username, password, out ec);
            } catch(ArgumentNullException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            }
            
            if (ec != ECode.None)
                throw new DataServerException(ec, Address, ERRMSG_QUERY + "AuthenticateCredentials");

            return value;
        }

        public int GetUserId(string username)
        {
            ECode ec = ECode.None;
            int value = 0;

            try
            {
                value = Server.GetUserId(username, out ec);
            } catch(ArgumentNullException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new DataServerException(ec, Address, ERRMSG_QUERY + "GetUserId");

            return value;
        }

        public User GetUser(int id)
        {
            ECode ec = ECode.None;
            User obj = null;

            try
            {
                obj = Server.GetUser(id, out ec);
            } catch(ArgumentNullException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new DataServerException(ec, Address, ERRMSG_QUERY + "GetUser");

            return obj;
        }

        public Hero GetHero(int id)
        {
            ECode ec = ECode.None;
            Hero obj = null;

            try
            {
                obj = Server.GetHero(id, out ec);
            } catch(ArgumentNullException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new DataServerException(ec, Address, ERRMSG_QUERY + "GetHero");

            return obj;
        }

        public Hero[] GetHeroTable()
        {
            ECode ec = ECode.None;
            Hero[] obj = null;

            try
            {
                obj = Server.GetHeroTable(out ec);
            } catch(ArgumentNullException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new DataServerException(ec, Address, ERRMSG_QUERY + "GetHeroTable");

            return obj;
        }

        public Boss GetBoss(int id)
        {
            ECode ec = ECode.None;
            Boss obj = null;

            try
            {
                obj = Server.GetBoss(id, out ec);
            } catch(ArgumentNullException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new DataServerException(ec, Address, ERRMSG_QUERY + "GetBoss");

            return obj;
        }

        public Boss[] GetBossTable()
        {
            ECode ec = ECode.None;
            Boss[] obj = null;

            try
            {
                obj = Server.GetBossTable(out ec);
            } catch(ArgumentNullException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(CommunicationException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            } catch(TimeoutException ex) {
                throw new DataServerException(ECode.ConnectionError, Address, ERRMSG_CONNECTION, ex);
            }

            if (ec != ECode.None)
                throw new DataServerException(ec, Address, ERRMSG_QUERY + "GetBossTable");

            return obj;
        }
        #endregion
    }
}

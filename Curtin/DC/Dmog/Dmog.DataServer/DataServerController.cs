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
    /// The DataServer server object
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    public class DataServerController : IDataServerController
    {
        ///// FIELDS /////////////////////////////////////
        private DatabaseController DBCon;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public DataServerController()
        {
            Console.WriteLine("Server object initiated!");
            try
            {
                DBCon = new DatabaseController();
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError,
                    "Failed to connect to database: database not found!", ex);
            }
        }

        ~DataServerController()
        {
            Console.WriteLine("Server object terminated!");
        }
        //////////////////////////////////////////////////
        
        #region "///// METADATA GETTERS /////"
        public int GetUserCount(out ECode ec)
        {
            int count = -1;
            ec = ECode.None;

            try
            {
                count = DBCon.QueryUserCount();
            } catch (DatabaseException ex) {
                ec = ex.ErrorCode;
            }
            return count;
        }

        public int GetHeroCount(out ECode ec)
        {
            int count = -1;
            ec = ECode.None;

            try
            {
                count = DBCon.QueryHeroCount();
            } catch (DatabaseException ex) {
                ec = ex.ErrorCode;
            }
            return count;
        }

        public int GetBossCount(out ECode ec)
        {
            int count = -1;
            ec = ECode.None;

            try
            {
                count = DBCon.QueryBossCount();
            } catch (DatabaseException ex) {
                ec = ex.ErrorCode;
            }
            return count;
        }
        #endregion
        
        #region "///// GAME DATA GETTERS /////"
        public int AuthenticateCredentials(string username, string password, out ECode ec)
        {
            int id = -1;
            ec = ECode.None;

            try
            {
                id = DBCon.QueryUserId(username);
                if (id != -1 && !DBCon.QueryUserPassword(id).Equals(password))
                    id = -1;

            } catch (DatabaseException ex) {
                ec = ex.ErrorCode;
            }
            return id;
        }

        public int GetUserId(string username, out ECode ec)
        {
            int id = -1;
            ec = ECode.None;

            try
            {
                id = DBCon.QueryUserId(username);
            } catch (DatabaseException ex) {
                ec = ex.ErrorCode;
            }
            return id;
        }

        public User GetUser(int id, out ECode ec)
        {
            User obj = null;
            ec = ECode.None;

            try
            {
                obj = DBCon.QueryUser(id);
            } catch (DatabaseException ex) {
                ec = ex.ErrorCode;
            }
            return obj;
        }

        public Hero GetHero(int id, out ECode ec)
        {
            Hero obj = null;
            ec = ECode.None;

            try
            {
                obj = DBCon.QueryHero(id);
            } catch (DatabaseException ex) {
                ec = ex.ErrorCode;
            }
            return obj;
        }

        public Hero[] GetHeroTable(out ECode ec)
        {
            Hero[] obj = null;
            ec = ECode.None;

            try
            {
                obj = DBCon.QueryHeroTable();
            } catch (DatabaseException ex) {
                ec = ex.ErrorCode;
            }
            return obj;
        }

        public Boss GetBoss(int id, out ECode ec)
        {
            Boss obj = null;
            ec = ECode.None;

            try
            {
                obj = DBCon.QueryBoss(id);
            } catch (DatabaseException ex) {
                ec = ex.ErrorCode;
            }
            return obj;
        }

        public Boss[] GetBossTable(out ECode ec)
        {
            Boss[] obj = null;
            ec = ECode.None;

            try
            {
                obj = DBCon.QueryBossTable();
            } catch (DatabaseException ex) {
                ec = ex.ErrorCode;
            }
            return obj;
        }
        #endregion
    }
}

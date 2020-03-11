/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.IO;
using System.Collections.Generic;
using DistributedGameDatabase;
using Dmog.ServerSupport;
using Dmog.GameObjects;

namespace Dmog.DataServer
{
    /// <summary>
    /// Represents the entry point into the database.
    /// Provides wrapper methods that add a layer of error handling and data abstraction.
    /// </summary>
    internal class DatabaseController
    {
        ///// FIELDS /////////////////////////////////////
        // Error Messages //
        private const string ERRMSG_CONNECTION = "Failed to connect to database: database not found!";
        private const string ERRMSG_QUERY = "Queried index out of range!";

        private DistributedGameDB Database;
        //////////////////////////////////////////////////

        /// <summary>
        /// Initialises the underlying database
        /// </summary>
        public DatabaseController()
        {
            try
            {
                Database = new DistributedGameDB();
                Database.InitDB();
            } catch (IndexOutOfRangeException ex) {
                throw new DatabaseException(ECode.ConnectionError, "Invalid user file format!", ex);
            } catch (FileNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            }
        }
        
        #region "///// METADATA QUERIES /////"
        public int QueryUserCount()
        {
            try {
                return Database.GetNumUsers();
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            }
        }

        public int QueryHeroCount()
        {
            try {
                return Database.GetNumHeroes();
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            }
        }

        public int QueryBossCount()
        {
            try {
                return Database.GetNumBosses();
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            }
        }
        #endregion

        #region "///// USER DATA QUERIES /////"
        public User QueryUser(int id)
        {
            User result = null;
            
            try {
                string username = QueryUserName(id);
                string[] friends = QueryUserFriends(id);
                if (username == null || friends == null)
                    throw new ArgumentNullException();

                result = new User(id, username, friends);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }

        public string QueryUserName(int id)
        {
            string result = null;

            try {
                Database.GetUsernamePassword(id, out result, out string password);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }

        public string QueryUserPassword(int id)
        {
            string result = null;

            try {
                Database.GetUsernamePassword(id, out string username, out result);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }
        
        public string[] QueryUserFriends(int id)
        {
            string[] result = null;

            try {
                result = Database.GetFriendsByID(id).ToArray();
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }


        public int QueryUserId(string username)
        {
            int userCount = QueryUserCount();
            string search = null;
            int i = 0, id = -1;

            try {
                do {
                    search = QueryUserName(i);
                    if (search != null && search.Equals(username))
                        id = i;
                    i++;
                } while (i < userCount && id == -1); // while theres more users && not found match

            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                id = -1;
            }

            return id;
        }
        #endregion

        #region "///// HERO DATA QUERIES /////"
        public Hero[] QueryHeroTable()
        {
            List<Hero> heroList = new List<Hero>();
            int heroCount = QueryHeroCount();

            for (int i = 0; i < heroCount; i++)
            {
                heroList.Add(QueryHero(i));
            }

            return heroList.ToArray();
        }

        public Hero QueryHero(int id)
        {
            Hero result = null;

            try {
                Database.GetHeroStatsByID(id, out int def, out int hp, out int moveNum);
                string name = QueryHeroName(id);
                Ability[] abilities = QueryHeroAbilities(id);
                if (name == null || abilities == null)
                    throw new ArgumentNullException();

                result = new Hero(id, name, Hero.TEAM, hp, def, abilities);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }

        public string QueryHeroName(int id)
        {
            string result = null;

            try {
                result = Database.GetHeroNameByID(id);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }

        public int QueryHeroDefence(int id)
        {
            int result = -1;

            try {
                Database.GetHeroStatsByID(id, out result, out int hp, out int moveNum);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = -1;
            }

            return result;
        }

        public int QueryHeroHp(int id)
        {
            int result = -1;

            try {
                Database.GetHeroStatsByID(id, out int def, out result, out int moveNum);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = -1;
            }

            return result;
        }

        public int QueryHeroAbilityCount(int id)
        {
            int result = -1;

            try {
                Database.GetHeroStatsByID(id, out int def, out int hp, out result);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = -1;
            }

            return result;
        }

        public Ability QueryHeroAbility(int id, int index)
        {
            Ability result = null;

            try {
                Database.GetMovesByIDAndIndex(id, index, out int value, out string description, out char type, out char target);
                result = new Ability(index, description, value, (AbilityEffect)type, TargetStrategy.Manual, (TargetType)target);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }
        
        public Ability[] QueryHeroAbilities(int id)
        {
            Ability[] result = null;

            try {
                int abilityCount = QueryHeroAbilityCount(id);
                result = new Ability[abilityCount];

                for (int i = 0; i < abilityCount; i++)
                {
                    result[i] = QueryHeroAbility(id, i);
                }
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }
        #endregion

        #region "///// BOSS DATA QUERIES /////"
        public Boss[] QueryBossTable()
        {
            List<Boss> bossList = new List<Boss>();
            int bossCount = QueryBossCount();

            for (int i = 0; i < bossCount; i++)
            {
                bossList.Add(QueryBoss(i));
            }

            return bossList.ToArray();
        }

        public Boss QueryBoss(int id)
        {
            Boss result = null;

            try {
                Database.GetBossStatsByID(id, out int def, out int hp, out int damage, out char targetPref);
                string name = QueryBossName(id);
                Ability[] ability = QueryBossAbilities(id);
                if (name == null || ability == null)
                    throw new ArgumentNullException();

                result = new Boss(id, name, Boss.TEAM, hp, def, ability);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }

        public string QueryBossName(int id)
        {
            string result = null;

            try {
                result = Database.GetBossNameByID(id);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }

        public int QueryBossDefence(int id)
        {
            int result = -1;

            try {
                Database.GetBossStatsByID(id, out result, out int hp, out int damage, out char targetPref);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = -1;
            }

            return result;
        }

        public int QueryBossHp(int id)
        {
            int result = -1;

            try {
                Database.GetBossStatsByID(id, out int def, out result, out int damage, out char targetPref);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = -1;
            }

            return result;
        }

        public int QueryBossAbilityCount(int id)
        {
            int result = -1;
            
            try {
                Database.GetBossNameByID(id); // check if boss id is valid
                result = 1;
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = -1;
            }

            return result;
        }

        public Ability QueryBossAbility(int id, int index)
        {
            Ability result = null;

            try {
                Database.GetBossStatsByID(id, out int def, out int hp, out int damage, out char targetPref);
                result = new Ability(index, "Boss Ability", damage, AbilityEffect.Damage, (TargetStrategy)targetPref, TargetType.Team);
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }

        public Ability[] QueryBossAbilities(int id)
        {
            Ability[] result = null;

            try {
                int abilityCount = QueryBossAbilityCount(id);
                result = new Ability[abilityCount];

                for (int i = 0; i < abilityCount; i++)
                {
                    result[i] = QueryBossAbility(id, i);
                }
            } catch (DllNotFoundException ex) {
                throw new DatabaseException(ECode.ConnectionError, ERRMSG_CONNECTION, ex);
            } catch (ArgumentOutOfRangeException) {
                result = null;
            }

            return result;
        }
        #endregion
    }
}

/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dmog.ServerSupport
{
    /// <summary>
    /// Provides a means of storing & managing a set of unique Client Sessions.
    /// Each client session is indexable by uniquely generated session token.
    /// </summary>
    public class ClientSessionRegistry : IEnumerable
    {
        ///// FIELDS /////////////////////////////////////
        /// <summary>
        /// An index of session-token, ClientSessions pairs.
        /// </summary>
        private Dictionary<string, ClientSession> Registry;

        /// <summary>
        /// An index of PUID, SessionToken pairs.
        /// Allows for fast lookup of Client-Sessions without knowing the token.
        /// </summary>
        private Dictionary<string, string> RegistryIndex;
        private int Salt; // Salt for generating session token
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public ClientSessionRegistry()
        {
            Registry = new Dictionary<string, ClientSession>();
            RegistryIndex = new Dictionary<string, string>();
            Salt = 0;
        }
        //////////////////////////////////////////////////

        public IEnumerator GetEnumerator()
        {
            return Registry.GetEnumerator();
        }

        /// <summary>
        /// Generates a session token
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private string GenerateToken(string seed)
        {
            Salt++; // Super secure hash function
            return (Salt.ToString() + "_" + seed);
        }
        
        public bool RegisterClient(string token, ClientSession session)
        {
            bool success = false;
            if (token == null || session == null || session.PUID == null)
                throw new ArgumentNullException("token or session or PUID cannot be null");
            
            string puid = session.PUID;

            if (IsPuidRegistered(puid) == false)
            {
                Registry.Add(token, session);
                RegistryIndex.Add(puid, token);
                success = true;
            }

            return success;
        }

        public string RegisterClient(ClientSession session)
        {
            if (session == null || session.PUID == null)
                throw new ArgumentNullException("session or PUID cannot be null");
            
            string token = null;
            string puid = session.PUID;
            
            if (IsPuidRegistered(puid) == false)
            {
                token = GenerateToken(puid);
                Registry.Add(token, session);
                RegistryIndex.Add(puid, token);
            }
            
            return token;
        }

        public void UnregisterClient(string token)
        {
            try
            {
                string puid = Registry[token].PUID;
                Registry.Remove(token);
                RegistryIndex.Remove(puid);
            } catch (ArgumentNullException) {
            } catch (KeyNotFoundException) {}
        }
        

        #region "///// QUERY METHODS /////"
        public int GetCount()
        {
            return Registry.Count;
        }

        public string GetToken(string puid)
        {
            string token = null;
            if (puid != null)
                RegistryIndex.TryGetValue(puid, out token);

            return token;
        }
        
        public bool IsPuidRegistered(string puid)
        {
            bool registered = false;
            if (puid != null)
                registered = RegistryIndex.ContainsKey(puid);

            return registered;
        }

        public bool IsTokenRegistered(string token)
        {
            bool registered = false;
            if (token != null)
                registered = Registry.ContainsKey(token);

            return registered;
        }

        public bool IsClientRegistered(string token)
        {
            bool registered = false;
            if (token != null)
            {
                ClientSession cs = GetClientSession(token);
                if (cs != null)
                    registered = IsPuidRegistered(cs.PUID);
            }
            return registered;
        }

        public bool IsClientRegistered(string token, string puid)
        {
            bool registered = false;
            if (token != null && puid != null)
            {
                ClientSession cs = GetClientSession(token);
                if (cs != null)
                    registered = (cs.PUID == puid);
            }
            return registered;
        }

        public ClientSession GetClientSession(string token)
        {
            ClientSession cs = null;

            try
            {
                cs = Registry[token];
            }
            catch (ArgumentNullException) {}
            catch (KeyNotFoundException) {}
            
            return cs;
        }
        
        public ClientSession[] GetClientSessionArray()
        {
            return Registry.Values.ToArray();
        }
        #endregion
    }

    /// <summary>
    /// Represents a client connection session
    /// </summary>
    public abstract class ClientSession
    {
        ///// FIELDS /////////////////////////////////////
        /// <summary>
        /// The Public Unique Identifier used to address the client.
        /// For Users, this would be the username.
        /// For game servers, this would be the server name.
        /// </summary>
        public string PUID { get; }
        public string ClientAddress { get; }
        public DateTime CreationDate { get; }
        public DateTime ExpirationDate { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public ClientSession(string puid, string clientAddress, double lifeSpan)
        {
            if (puid == null)
                throw new ArgumentNullException("UID cannot be null");
            if (clientAddress == null)
                throw new ArgumentNullException("Address cannot be null");
            if (lifeSpan < 1)
                lifeSpan = 1;

            PUID = puid;
            ClientAddress = clientAddress;
            CreationDate = DateTime.Now;
            ExpirationDate = CreationDate.AddMinutes(lifeSpan);
        }
        //////////////////////////////////////////////////
        
        /// <summary>
        /// Extends the client session lifespan.
        /// </summary>
        /// <param name="mins">Number of minutes to add to lifespan</param>
        public void RenewLifeSpan(double mins)
        {
            if (mins < 1)
                mins = 1;

            ExpirationDate = DateTime.Now.AddMinutes(mins);
        }
    }
}

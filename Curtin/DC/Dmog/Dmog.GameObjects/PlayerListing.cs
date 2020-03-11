/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dmog.GameObjects
{
    /// <summary>
    /// Stores a collection of PlayerDetail objects
    /// </summary>
    [DataContract]
    public class PlayerListing
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public List<PlayerDetail> PlayerDetailList { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public PlayerListing()
        {
            PlayerDetailList = new List<PlayerDetail>();
        }

        public PlayerListing(List<PlayerDetail> pdList)
        {
            PlayerDetailList = pdList;
        }
        //////////////////////////////////////////////////
    }

    /// <summary>
    /// Stores information about a player for transmission over a network.
    /// Used for update/read-only purposes only.
    /// </summary>
    [DataContract]
    public class PlayerDetail
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public string Username { get; set; }
        [DataMember] public Hero PlayerHero { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public PlayerDetail(string username, Hero playerHero)
        {
            Username = username;
            PlayerHero = playerHero;
        }
        //////////////////////////////////////////////////
    }
}

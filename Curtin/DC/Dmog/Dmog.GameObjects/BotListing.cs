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
    /// Stores a collection of BotDetail objects
    /// </summary>
    [DataContract]
    public class BotListing
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public List<BotDetail> BotDetailList { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public BotListing()
        {
            BotDetailList = new List<BotDetail>();
        }

        public BotListing(List<BotDetail> bdList)
        {
            BotDetailList = bdList;
        }
        //////////////////////////////////////////////////
    }

    /// <summary>
    /// Stores information about a bot for transmission over a network.
    /// Used for update/read-only purposes only.
    /// </summary>
    [DataContract]
    public class BotDetail
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public string Username { get; set; }
        [DataMember] public Boss BotBoss { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public BotDetail(string username, Boss botBoss)
        {
            Username = username;
            BotBoss = botBoss;
        }
        //////////////////////////////////////////////////
    }
}

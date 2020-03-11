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
    /// Stores a collection of GameDetail objects
    /// </summary>
    [DataContract]
    public class GameListing
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public List<GameDetail> GameServerDetailList { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public GameListing(List<GameDetail> gsdList)
        {
            GameServerDetailList = gsdList;
        }
        //////////////////////////////////////////////////
    }

    /// <summary>
    /// Stores information about a game server for transmission over a network.
    /// Used for update/read-only purposes only.
    /// </summary>
    [DataContract]
    public class GameDetail
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public string ServerName { get; set; }
        [DataMember] public string ServerAddress { get; set; }
        [DataMember] public List<string> PlayerList { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public GameDetail(string serverName, string serverAddress, List<string> playerList)
        {
            ServerName = serverName;
            ServerAddress = serverAddress;
            PlayerList = playerList;
        }
        //////////////////////////////////////////////////
    }
}

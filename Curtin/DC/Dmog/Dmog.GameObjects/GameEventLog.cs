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
    /// Stores a collection of GameEvent objects
    /// </summary>
    [DataContract]
    public class GameEventLog
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public List<GameEvent> GameEventList { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public GameEventLog()
        {
            GameEventList = new List<GameEvent>();
        }

        public GameEventLog(List<GameEvent> geList)
        {
            GameEventList = geList;
        }
        //////////////////////////////////////////////////
    }

    /// <summary>
    /// Stores information about an in game event for transmission over a network.
    /// Used for update/read-only purposes only.
    /// </summary>
    [DataContract]
    public class GameEvent
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public int Round { get; set; }
        [DataMember] public string Actor { get; set; }
        [DataMember] public string[] Targets { get; set; }
        [DataMember] public AbilityEffect AbilityEffect { get; set; }
        [DataMember] public int EffectMagnitude { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public GameEvent(int round, string actor, string[] targets,
                         AbilityEffect abilityEffect, int effectMagnitude)
        {
            Round = round;
            Actor = actor;
            Targets = targets;
            AbilityEffect = abilityEffect;
            EffectMagnitude = effectMagnitude;
        }
        //////////////////////////////////////////////////
    }
}

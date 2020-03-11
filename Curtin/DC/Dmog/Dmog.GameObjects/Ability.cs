/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace Dmog.GameObjects
{
    /// <summary>
    /// Represents an Ability for an Entity (Hero or Boss)
    /// </summary>
    [DataContract]
    public class Ability
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public int Id { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public int Value { get; set; }

        [DataMember] public AbilityEffect AbilityEffect { get; set; }
        [DataMember] public TargetStrategy TargetStrategy { get; set; }
        [DataMember] public TargetType TargetType { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public Ability(int id, string description, int value,
            AbilityEffect abilityEffect, TargetStrategy targetStrategy, TargetType targetType)
        {
            this.Id = id;
            this.Description = description;
            this.Value = value;
            this.AbilityEffect = abilityEffect;
            this.TargetStrategy = targetStrategy;
            this.TargetType = targetType;
        }
        //////////////////////////////////////////////////
    }
}

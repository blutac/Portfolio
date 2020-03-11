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
    /// The base class for the Hero & Boss classes
    /// </summary>
    [DataContract]
    public abstract class Entity
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public int Id { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Team { get; set; }
        [DataMember] public int HpMax { get; set; }
        [DataMember] public int HpCurrent { get; set; }
        [DataMember] public int Defence { get; set; }
        [DataMember] public Ability[] Abilities { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public Entity(int id, string name, string team, int hp, int defence, Ability[] abilities)
        {
            this.Id = id;
            this.Name = name;
            this.Team = team;
            this.HpMax = hp;
            this.HpCurrent = hp;
            this.Defence = defence;
            this.Abilities = abilities;
        }
        //////////////////////////////////////////////////
    }
}

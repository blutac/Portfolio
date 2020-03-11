/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace Dmog.GameObjects
{
    [DataContract]
    public class Hero : Entity
    {
        ///// FIELDS /////////////////////////////////////
        /// <summary>
        /// The default team for heroes.
        /// For use in preventing friendly fire in combat.
        /// </summary>
        public static readonly string TEAM = "HERO";
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public Hero(int id, string name, string team, int hp, int defence, Ability[] abilities)
            : base(id, name, team, hp, defence, abilities)
        {
        }

        public Hero(Hero hero)
            : base(hero.Id, hero.Name, hero.Team, hero.HpMax, hero.Defence, hero.Abilities)
        {
        }
        //////////////////////////////////////////////////
    }
}

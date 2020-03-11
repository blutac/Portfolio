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
    public class Boss : Entity
    {
        ///// FIELDS /////////////////////////////////////
        /// <summary>
        /// The default public unique id for bosses.
        /// For use in addressing the boss in combat.
        /// </summary>
        public static readonly string PUID = "[ BOSS ]";
        /// <summary>
        /// The default team for bosses.
        /// For use in preventing friendly fire in combat.
        /// </summary>
        public static readonly string TEAM = "BOSS";
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public Boss(int id, string name, string team, int hp, int defence, Ability[] abilities)
            : base(id, name, team, hp, defence, abilities)
        {
        }

        public Boss(Boss boss)
            : base(boss.Id, boss.Name, boss.Team, boss.HpMax, boss.Defence, boss.Abilities)
        {
        }
        //////////////////////////////////////////////////
    }
}

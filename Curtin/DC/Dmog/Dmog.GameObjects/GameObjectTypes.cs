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
    /// Specifies what effect the ability has
    /// </summary>
    public enum AbilityEffect
    {
        /// <summary>
        /// The ability increases Hp
        /// </summary>
        Heal = 'H',

        /// <summary>
        /// The ability decreases Hp
        /// </summary>
        Damage = 'D'
    }

    /// <summary>
    /// Specifies how the target is selected
    /// </summary>
    public enum TargetStrategy
    {
        /// <summary>
        /// Target is chosen via selection
        /// </summary>
        Manual = 0,
        /// <summary>
        /// Target is chosen randomly
        /// </summary>
        Random = 'R',
        /// <summary>
        /// Highest hitting target is chosen
        /// </summary>
        HighestHitting = 'H'
    }

    /// <summary>
    /// Specifies what you can target
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// Target is a single entity (i.e. Boss or Hero)
        /// </summary>
        Single = 'S',
        /// <summary>
        /// Target is a team of entities (i.e. a group of Heros or Bosses on the same team)
        /// </summary>
        Team = 'M'
    }
}

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
    /// Stores a player's move instructions for transmission over a network.
    /// </summary>
    [DataContract]
    public class PlayerMove
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public int SelectedAbilityId { get; set; }
        [DataMember] public string SelectedTarget { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public PlayerMove(int selectedAbilityId, string selectedTarget)
        {
            SelectedAbilityId = selectedAbilityId;
            SelectedTarget = selectedTarget;
        }
        //////////////////////////////////////////////////
    }
}

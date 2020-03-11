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
    public class User
    {
        ///// FIELDS /////////////////////////////////////
        [DataMember] public int Id { get; set; }
        [DataMember] public string Username { get; set; }

        /// <summary> a collection of usernames </summary>
        [DataMember] public string[] Friends { get; set; }
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public User(int id, string username, string[] friends)
        {
            this.Id = id;
            this.Username = username;
            this.Friends = friends;
        }
        //////////////////////////////////////////////////
    }
}

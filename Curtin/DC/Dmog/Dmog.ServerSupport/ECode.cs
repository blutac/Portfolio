/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////

namespace Dmog.ServerSupport
{
    /// <summary>
    /// Provides error codes for transmission of error states between Dmog server/clients
    /// </summary>
    public enum ECode
    {
        /// <summary>
        /// No error state
        /// </summary>
        None = 0,

        /// <summary>
        /// An connection attempt to a server failed
        /// </summary>
        ConnectionError = 1,

        /// <summary>
        /// There was a problem with the Query or Request
        /// </summary>
        QueryError = 2,

        /// <summary>
        /// The provided client session token failed
        /// </summary>
        AuthenticationFail = 3,

        /// <summary>
        /// The server is full
        /// </summary>
        ServerFull = 4
    }
}

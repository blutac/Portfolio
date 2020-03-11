/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;

namespace Dmog.ServerSupport
{
    /// <summary>
    /// Thrown by the ServerConnection object when it fails to connect to the server.
    /// </summary>
    public class ServerConnectionException : Exception
    {
        ///// FIELDS /////////////////////////////////////
        public readonly ECode ErrorCode;
        public readonly string Address;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public ServerConnectionException(ECode errorCode, string address, string message)
            : base(message)
        {
            ErrorCode = errorCode;
            Address = address;
        }
        public ServerConnectionException(ECode errorCode, string address, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            Address = address;
        }
        //////////////////////////////////////////////////
    }
}
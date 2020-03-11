/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using Dmog.ServerSupport;

namespace Dmog.PortalServer
{
    public class PortalServerException : Exception
    {
        ///// FIELDS /////////////////////////////////////
        public readonly ECode ErrorCode;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public PortalServerException(ECode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
        public PortalServerException(ECode errorCode, string message, Exception inner) : base(message, inner)
        {
            ErrorCode = errorCode;
        }
        //////////////////////////////////////////////////
    }
}

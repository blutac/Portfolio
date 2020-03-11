/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using Dmog.ServerSupport;

namespace Dmog.DataServer
{
    public class DataServerException : Exception
    {
        ///// FIELDS /////////////////////////////////////
        public readonly ECode ErrorCode;
        public readonly string Address;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public DataServerException(ECode errorCode, string address, string message) : base(message)
        {
            ErrorCode = errorCode;
            Address = address;
        }
        public DataServerException(ECode errorCode, string address, string message, Exception inner) : base(message, inner)
        {
            ErrorCode = errorCode;
            Address = address;
        }
        //////////////////////////////////////////////////
    }
}

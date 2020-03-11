using System;
using Dmog.ServerSupport;

namespace Dmog.DataServer
{
    public class DatabaseException : Exception
    {
        ///// FIELDS /////////////////////////////////////
        public readonly ECode ErrorCode;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public DatabaseException(ECode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
        public DatabaseException(ECode errorCode, string message, Exception inner) : base(message, inner)
        {
            ErrorCode = errorCode;
        }
        //////////////////////////////////////////////////
    }
}

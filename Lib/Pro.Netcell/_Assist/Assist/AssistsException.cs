using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Netcell
{
    [Serializable]
    public class AssistsException : Exception
    {
        public const string SessionKeyPrevent = "לא ניתן להמשיך בפעולה זו , אנא פנה לתמיכה";

        public const string SessionCreationError = SessionKeyPrevent + "-1001";
        public const string InvalidSessionItem = SessionKeyPrevent + "-1002";
        public const string AddSessionItemError = SessionKeyPrevent + "-1003";



        public AssistsException(string msg)
            : base(msg)
        {
        }

        public AssistsException(string msg, Exception innerExeption)
            : base(msg, innerExeption)
        {
        }
        public AssistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }

     [Serializable]
    public class SessionException : Exception
    {
        public SessionException(string msg)
            : base(msg)
        {
        }

        public SessionException(string msg, EntryPointNotFoundException innerExeption)
            : base(msg, innerExeption)
        {
        }

        public SessionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
     [Serializable]
    public class UnexpectedException : Exception
    {
        public UnexpectedException(string msg)
            : base(msg)
        {
        }

        public UnexpectedException(string msg, EntryPointNotFoundException innerExeption)
            : base(msg, innerExeption)
        {
        }
        public UnexpectedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
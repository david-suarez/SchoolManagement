using System;
using System.Runtime.Serialization;

namespace SchoolManagement.Exceptions
{
    public class QueryNotMatchException : Exception
    {
        public QueryNotMatchException()
        {
        }

        public QueryNotMatchException(string message) : base(message)
        {
        }

        public QueryNotMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QueryNotMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

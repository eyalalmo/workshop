using System;
using System.Runtime.Serialization;

namespace workshop192.Domain
{
    [Serializable]
    public class UserException : ClientException
    {
        public UserException()
        {
        }

        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
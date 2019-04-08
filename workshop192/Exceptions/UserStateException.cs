using System;
using System.Runtime.Serialization;

namespace workshop192.Domain
{
    [Serializable]
    internal class UserStateException : Exception
    {
        public UserStateException()
        {
        }

        public UserStateException(string message) : base(message)
        {
        }

        public UserStateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserStateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
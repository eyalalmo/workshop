using System;
using System.Runtime.Serialization;

namespace workshop192.Domain
{
    [Serializable]
    public class IllegalNameException : Exception
    {
        public IllegalNameException()
        {
        }

        public IllegalNameException(string message) : base(message)
        {
        }

        public IllegalNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
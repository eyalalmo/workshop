using System;
using System.Runtime.Serialization;

namespace workshop192.Domain
{
    [Serializable]
    internal class AlreadyExistException : Exception
    {
        public AlreadyExistException()
        {
        }

        public AlreadyExistException(string message) : base(message)
        {
        }

        public AlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
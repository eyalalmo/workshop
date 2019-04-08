using System;
using System.Runtime.Serialization;

namespace workshop192.Domain
{
    [Serializable]
    internal class RoleException : Exception
    {
        public RoleException()
        {
        }

        public RoleException(string message) : base(message)
        {
        }

        public RoleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RoleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
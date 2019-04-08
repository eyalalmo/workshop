using System;
using System.Runtime.Serialization;

namespace workshop192.Domain
{
    [Serializable]
    internal class PermissionsException : Exception
    {
        public PermissionsException()
        {
        }

        public PermissionsException(string message) : base(message)
        {
        }

        public PermissionsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PermissionsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
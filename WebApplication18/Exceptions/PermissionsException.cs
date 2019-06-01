using System;
using System.Runtime.Serialization;

namespace workshop192.Domain
{
    [Serializable]
    public class PermissionsException : ClientException
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
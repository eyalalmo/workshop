using System;
using System.Runtime.Serialization;

namespace workshop192.Domain
{
    [Serializable]
    internal class StoreException : Exception
    {
        public StoreException()
        {
        }

        public StoreException(string message) : base(message)
        {
        }

        public StoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
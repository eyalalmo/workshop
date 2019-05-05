using System;
using System.Runtime.Serialization;

namespace workshop192.Domain
{
    [Serializable]
    public class CartException : Exception
    {
        public CartException()
        {
        }

        public CartException(string message) : base(message)
        {
        }

        public CartException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CartException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace workshop192.Domain
{
    [Serializable]
    internal class IllegalAmountException : Exception
    {
        public IllegalAmountException()
        {
        }

        public IllegalAmountException(string message) : base(message)
        {
        }

        public IllegalAmountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalAmountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
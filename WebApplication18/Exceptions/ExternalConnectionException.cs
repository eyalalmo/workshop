using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApplication18.Exceptions
{
    public class ExternalConnectionException : Exception
    {
            public ExternalConnectionException()
            {
            }

            public ExternalConnectionException(string message) : base(message)
            {
            }

            public ExternalConnectionException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ExternalConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
       
    }
}
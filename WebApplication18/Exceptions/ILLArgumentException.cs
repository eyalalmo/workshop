using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace workshop192.Domain
{
 
        [Serializable]
        public class ILLArgumentException : Exception
        {
            public ILLArgumentException()
            {
            }

            public ILLArgumentException(string message) : base(message)
            {
            }

            public ILLArgumentException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ILLArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

    }

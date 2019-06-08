using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using workshop192.Domain;

namespace workshop192.Domain
{
    

        [Serializable]
        public class SuccessPaymentExeption : ClientException
        {
            public SuccessPaymentExeption()
            {
            }

            public SuccessPaymentExeption(string message) : base(message)
            {
            }

            public SuccessPaymentExeption(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected SuccessPaymentExeption(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }

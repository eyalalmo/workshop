using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class PaymentService
    {
        
        private static PaymentService instance = null;

        private PaymentService()
        {
           

        }

        public static PaymentService getInstance()
        {
            if (instance == null)
            {
                instance = new PaymentService();
            }
            return instance;
        }

        public bool checkOut(String account ,int money)
        {
            return true;
        }
        public bool cancelPayment(String account, int money)
        {
            return true;
        }

    }
}

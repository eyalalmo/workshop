using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class DeliveryService
    {

        private static DeliveryService instance = null;

        private DeliveryService()
        {


        }

        public static DeliveryService getInstance()
        {
            if (instance == null)
            {
                instance = new DeliveryService();
            }
            return instance;
        }

        public Boolean sendToUser(String address, Product p)
        {
            return true;
        }

        public bool connectToSystem()
        {
            return true;
        }
    }
}

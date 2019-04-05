using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class System
    {
        public System()
        {
            init();
        }

        private void init()
        {
            SubscribedUser admin = new SubscribedUser("Admin", "1234", new ShoppingBasket());
            DBSubscribedUser.getInstance().register(admin);
            PaymentService.getInstance().connectToSystem();
            DeliveryService.getInstance().connectToSystem();
            ConsistencySystem.getInstance().connectToSystem();
        }
    }
}

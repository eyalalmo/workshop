using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Bridge;
using workshop192.ServiceLayer;

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
            Session s = new Session();
            s.register("Admin", "1234");
            s.register("storeOwner", "123");
            s.login("storeOwner", "123");
            int sID = DomainBridge.getInstance().createStore(s, "startup", "This is the startup Store!");
            DomainBridge.getInstance().addProduct("new Pro1", "Pros",5, 0, 10, sID, s);
            DomainBridge.getInstance().addProduct("new Pro2", "Pros2", 7, 0, 8, sID, s);
            //SubscribedUser admin = new SubscribedUser("Admin", "1234", new ShoppingBasket());
            //Store s = new Store("startup", "This is the startup Store!");
            //DBStore.getInstance().addStore(s);
            //Product p = new Product("pro", "pros", 10, 0, 10, s);
            //s.addProduct(p);
            //DBProduct.getInstance().addProduct(p);
            PaymentService.getInstance().connectToSystem();
            DeliveryService.getInstance().connectToSystem();
            ConsistencySystem.getInstance().connectToSystem();
        }
    }
}

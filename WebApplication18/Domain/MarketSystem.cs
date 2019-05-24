using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication18.Domain;
using workshop192.Bridge;
using workshop192.ServiceLayer;

namespace workshop192.Domain
{
    public class MarketSystem
    {
        public static void init()
        {
            DBProduct.getInstance().init();
            DBSession.getInstance().init();
            DBStore.getInstance().init();
         //   DBComplaint.getInstance().init();
            DBDiscount.getInstance().init();
            DBSubscribedUser.getInstance().init();
            //    DBCookies.getInstance().init();

            int sessionid = DBSession.getInstance().generate();
            Session s = DBSession.getInstance().getSession(sessionid);
           
            s.register("et", "123");

            DBNotifications.getInstance().init();

            s.login("et", "123");
            int sID = DomainBridge.getInstance().createStore(sessionid, "startup", "This is the startup Store!");
            DomainBridge.getInstance().addProduct("new Pro1", "Pros",5, 5, 10, sID, sessionid);
            DomainBridge.getInstance().addProduct("new Pro2", "Pros2", 7, 2, 8, sID, sessionid);
            //SubscribedUser admin = new SubscribedUser("Admin", "1234", new ShoppingBasket());
            //Store s = new Store("startup", "This is the startup Store!");
            //DBStore.getInstance().addStore(s);
            //Product p = new Product("pro", "pros", 10, 0, 10, s);
            //s.addProduct(p);
            //DBProduct.getInstance().addProduct(p);
            s.logout();
            PaymentService.getInstance().connectToSystem();
            DeliveryService.getInstance().connectToSystem();
            ConsistencySystem.getInstance().connectToSystem();
            NotificationsBridge.getInstance().setObserver(DomainBridge.getInstance());
        }
    }
}

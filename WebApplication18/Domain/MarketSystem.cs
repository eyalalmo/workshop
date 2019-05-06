﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Bridge;
using workshop192.ServiceLayer;

namespace workshop192.Domain
{
    public class MarketSystem
    {

        private static MarketSystem instance;

        public static MarketSystem getInstance()
        {
            if (instance == null)
                instance = new MarketSystem();
            return instance;
        }

        private MarketSystem()
        {
            init();
        }

        private void init()
        {
            int sessionid = DBSession.getInstance().generate();
            Session s = DBSession.getInstance().getSession(sessionid);
            s.register("Admin", "1234");
            s.register("storeOwner", "123");
            s.login("storeOwner", "123");
            int sID = DomainBridge.getInstance().createStore(sessionid, "startup", "This is the startup Store!");
            DomainBridge.getInstance().addProduct("new Pro1", "Pros",5, 0, 10, sID, sessionid);
            DomainBridge.getInstance().addProduct("new Pro2", "Pros2", 7, 0, 8, sID, sessionid);
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

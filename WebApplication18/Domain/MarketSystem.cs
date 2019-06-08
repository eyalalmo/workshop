using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApplication18.Domain;
using WebApplication18.Logs;
using workshop192.Bridge;
using workshop192.ServiceLayer;

namespace workshop192.Domain
{
    public class MarketSystem
    {
        public static void init()
        {
            int doIt = 1;

            //int sessionid =0;
            //Session s=new Session();
            // int sID = 0;
            if (IsTestsMode.isTest == true)
            {
                SystemLogger.configureLogs();
                DBProduct.getInstance().initTests();
                DBSession.getInstance().initTests();
                DBDiscount.getInstance().initTests();
                DBSubscribedUser.getInstance().initTests();
                DBStore.getInstance().initTests();
                DBNotifications.getInstance().initTests();
                PaymentService.getInstance().connectToSystem();
                DeliveryService.getInstance().connectToSystem();
                ConsistencySystem.getInstance().connectToSystem();
                NotificationsBridge.getInstance().setObserver(DomainBridge.getInstance());
                return;
            }
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);

            string[] lines = File.ReadAllLines(filePath + "/input.txt");

            if (doIt == 1)
            {
                DBProduct.getInstance().init();
                DBSession.getInstance().init();
                DBDiscount.getInstance().init();
                DBSubscribedUser.getInstance().init();
                DBStore.getInstance().init();
                DBSubscribedUser.getInstance().updateShoppingBasket();
                DBNotifications.getInstance().init();
                PaymentService.getInstance().connectToSystem();
                DeliveryService.getInstance().connectToSystem();
                ConsistencySystem.getInstance().connectToSystem();
                NotificationsBridge.getInstance().setObserver(DomainBridge.getInstance());
                return;
            }
            foreach (string line in lines)
            {
                string[] input = line.Split(' ');
                if (input[0] == "createSession")
                {
                    int sessionid = DBSession.getInstance().generate();
                    DBSession.getInstance().getSession(sessionid);
                }
                else if (input[0] == "register")
                {
                    Session s = DBSession.getInstance().getSession(Int32.Parse(input[3]));
                    s.register(input[1], input[2]);
                }
                else if (input[0] == "init")
                {
                    DBProduct.getInstance().init();
                    DBSession.getInstance().init();
                    DBStore.getInstance().init();
                    DBDiscount.getInstance().init();
                    DBSubscribedUser.getInstance().init();
                    DBNotifications.getInstance().init();
                    PaymentService.getInstance().connectToSystem();
                    DeliveryService.getInstance().connectToSystem();
                    ConsistencySystem.getInstance().connectToSystem();
                }
                else if (input[0] == "login")
                {
                    Session s = DBSession.getInstance().getSession(Int32.Parse(input[3]));
                    s.login(input[1], input[2]);
                }
                else if (input[0] == "createStore")
                {
                    DomainBridge.getInstance().createStore(Int32.Parse(input[3]), input[1], input[2]);
                }
                else if (input[0] == "addProduct")
                {
                    DomainBridge.getInstance().addProduct(input[1], input[2], Int32.Parse(input[3]), Int32.Parse(input[4]), Int32.Parse(input[5]), Int32.Parse(input[6]), Int32.Parse(input[7]));
                }
                else if (input[0] == "logout")
                {
                    Session s = DBSession.getInstance().getSession(Int32.Parse(input[1]));
                    s.logout();
                }
                else if (input[0] == "addManager")
                {
                    DomainBridge.getInstance().addManager(Int32.Parse(input[1]), input[2], Boolean.Parse(input[3]), Boolean.Parse(input[4]), Boolean.Parse(input[5]), Int32.Parse(input[6]));
                }
                else if (input[0] == "addOwner")
                {
                    DomainBridge.getInstance().addOwner(Int32.Parse(input[1]), input[2], Int32.Parse(input[3]));
                }
                else if (input[0] == "setMaxAmountPolicy")
                {
                    DomainBridge.getInstance().setMaxAmountPolicy(Int32.Parse(input[1]), Int32.Parse(input[2]), Int32.Parse(input[3]));
                }
                else if (input[0] == "setMinAmountPolicy")
                {
                    DomainBridge.getInstance().setMinAmountPolicy(Int32.Parse(input[1]), Int32.Parse(input[2]), Int32.Parse(input[3]));
                }
                else if (input[0] == "setProductDiscount")
                {
                    DomainBridge.getInstance().setProductDiscount(Int32.Parse(input[1]), Int32.Parse(input[2]), Int32.Parse(input[3]));
                }
                else if (input[0] == "addToCart")
                {
                    DomainBridge.getInstance().addToCart(Int32.Parse(input[1]), Int32.Parse(input[2]), Int32.Parse(input[3]));
                }
                else if (input[0] == "setProductDiscount")
                {
                    DomainBridge.getInstance().setProductDiscount(Int32.Parse(input[1]), Int32.Parse(input[2]), Int32.Parse(input[3]));
                }
                else if (input[0] == "addStoreVisibleDiscount")
                {
                    DomainBridge.getInstance().addStoreVisibleDiscount(Int32.Parse(input[1]), Double.Parse(input[2]), input[3], Int32.Parse(input[4]));
                }
                else if (input[0] == "addProductVisibleDiscount")
                {
                    DomainBridge.getInstance().addProductVisibleDiscount(Int32.Parse(input[1]), Double.Parse(input[2]), input[3], Int32.Parse(input[4]));
                }
                else if (input[0] == "addAdmin")
                {
                    DomainBridge.getInstance().addAdmin(input[1], input[2]);
                }
                else if (input[0] == "removeUser")
                {
                    Session s = DBSession.getInstance().getSession(Int32.Parse(input[1]));
                    //session id, username
                    DomainBridge.getInstance().removeUser(Int32.Parse(input[1]), input[2]);
                }
                else if (input[0] == "addSession")
                {
                    DomainBridge.getInstance().addSession(input[0], Int32.Parse(input[1]));
                }
                else if (input[0] == "addToShoppingBasket")
                {
                    DomainBridge.getInstance().addToShoppingBasket(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));
                }
                else if (input[0] == "setProductRank")
                {
                    DomainBridge.getInstance().setProductRank(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));
                }
                else if (input[0] == "removeProduct")
                {
                    DomainBridge.getInstance().removeProduct(Int32.Parse(input[0]), Int32.Parse(input[1]));
                }
                else if (input[0] == "setProductPrice")
                {
                    DomainBridge.getInstance().setProductPrice(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));
                }
                else if (input[0] == "setProductName")
                {
                    DomainBridge.getInstance().setProductName(Int32.Parse(input[0]), input[1], Int32.Parse(input[2]));

                }
                else if (input[0] == "addToProductQuantity")
                {
                    DomainBridge.getInstance().addToProductQuantity(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));

                }
                else if (input[0] == "decFromProductQuantity")
                {
                    DomainBridge.getInstance().decFromProductQuantity(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));
                }
                else if (input[0] == "closeStore")
                {
                    DomainBridge.getInstance().closeStore(Int32.Parse(input[0]), Int32.Parse(input[1]));

                }
                else if (input[0] == "addManager")
                {
                    DomainBridge.getInstance().addManager(Int32.Parse(input[0]), input[1], Convert.ToBoolean(input[2]), Convert.ToBoolean(input[3]), Convert.ToBoolean(input[4]), Int32.Parse(input[5]));
                }
                else if (input[0] == "removeRole")
                {
                    DomainBridge.getInstance().removeRole(Int32.Parse(input[0]), input[1], Int32.Parse(input[2]));
                }
                else if (input[0] == "addToCart")
                {
                    DomainBridge.getInstance().addToCart(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));
                }
                else if (input[0] == "removeFromCart")
                {
                    DomainBridge.getInstance().removeFromCart(Int32.Parse(input[0]), Int32.Parse(input[1]));
                }
                else if (input[0] == "changeQuantity")
                {
                    DomainBridge.getInstance().changeQuantity(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));
                }
                else if (input[0] == "removeProductDiscount")
                {
                    DomainBridge.getInstance().removeProductDiscount(Int32.Parse(input[0]), Int32.Parse(input[1]));
                }
                else if (input[0] == "addStoreVisibleDiscount")
                {
                    DomainBridge.getInstance().addStoreVisibleDiscount(Int32.Parse(input[0]), Double.Parse(input[1]), input[2], Int32.Parse(input[3]));
                }
                else if (input[0] == "addProductVisibleDiscount")
                {
                    DomainBridge.getInstance().addProductVisibleDiscount(Int32.Parse(input[0]), Double.Parse(input[1]), input[2], Int32.Parse(input[3]));
                }
                else if (input[0] == "addReliantdiscountSameProduct")
                {
                    DomainBridge.getInstance().addReliantdiscountSameProduct(Int32.Parse(input[0]), Int32.Parse(input[1]), Double.Parse(input[2]), Int32.Parse(input[3]), input[4], Int32.Parse(input[4]));
                }
                else if (input[0] == "addReliantdiscountTotalAmount")
                {
                    DomainBridge.getInstance().addReliantdiscountTotalAmount(Int32.Parse(input[0]), Double.Parse(input[1]), Int32.Parse(input[2]), input[3], Int32.Parse(input[4]));
                }
                else if (input[0] == "removeStoreDiscount")
                {
                    DomainBridge.getInstance().removeStoreDiscount(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));
                }
                else if (input[0] == "setProductDiscount")
                {
                    DomainBridge.getInstance().setProductDiscount(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));
                }
                else if (input[0] == "complexDiscount")
                {
                    DomainBridge.getInstance().complexDiscount(input[0], Int32.Parse(input[1]), input[2], Double.Parse(input[3]), input[4], Int32.Parse(input[5]));
                }
                else if (input[0] == "removeMaxAmountPolicy")
                {
                    DomainBridge.getInstance().removeMaxAmountPolicy(Int32.Parse(input[0]), Int32.Parse(input[1]));
                }
                else if (input[0] == "removeMinAmountPolicy")
                {
                    DomainBridge.getInstance().removeMinAmountPolicy(Int32.Parse(input[0]), Int32.Parse(input[1]));
                }
                else if (input[0] == "setMinAmountPolicy")
                {
                    DomainBridge.getInstance().setMinAmountPolicy(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));
                }
                else if (input[0] == "setMaxAmountPolicy")
                {
                    DomainBridge.getInstance().setMaxAmountPolicy(Int32.Parse(input[0]), Int32.Parse(input[1]), Int32.Parse(input[2]));

                }
                else if (input[0] == "addPendingOwner")
                {
                    DomainBridge.getInstance().addPendingOwner(Int32.Parse(input[0]), input[1], Int32.Parse(input[2]));
                }

            }



        }

        //  int sessionid = DBSession.getInstance().generate();
        //Session s = DBSession.getInstance().getSession(sessionid);

        //  s.register("et", "123");

        //  DBNotifications.getInstance().init();

        //  s.login("et", "123");
        //   int sID = DomainBridge.getInstance().createStore(sessionid, "startup", "This is the startup Store!");
        //   DomainBridge.getInstance().addProduct("new Pro1", "Pros",5, 5, 10, sID, sessionid);
        //  DomainBridge.getInstance().addProduct("new Pro2", "Pros2", 7, 2, 8, sID, sessionid);

        // s.logout();
        // PaymentService.getInstance().connectToSystem();
        // DeliveryService.getInstance().connectToSystem();
        // ConsistencySystem.getInstance().connectToSystem();

    }
}
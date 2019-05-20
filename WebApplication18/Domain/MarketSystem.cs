using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
           
            //////////////////////////


            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"\input.txt");
            string[] lines = File.ReadAllLines(path);
            //    string[] lines = System.IO.File.ReadAllLines("/input.txt");

            int sessionid=0;
            Session s=new Session();
            int sID = 0;

               foreach (string line in lines)
              {
              string[] input=  line.Split(' ');
                if (input[0] == "createSession")
                {
                    sessionid = DBSession.getInstance().generate();
                    s = DBSession.getInstance().getSession(sessionid);
                }
                else if(input[0] == "register")
                {
                    s.register(input[1], input[2]);
                }
                else if(input[0] == "init")
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
                else if  (input[0] == "login")
                {
                    s.login(input[1], input[2]);
                }
                else if (input[0] == "createStore")
                {
                    sID= DomainBridge.getInstance().createStore(sessionid,input[1], input[2]);
                }
                else if (input[0] == "  ")
                {
                    DomainBridge.getInstance().addProduct( input[1], input[2], Int32.Parse(input[3]), Int32.Parse(input[4]), Int32.Parse(input[5]), sID,sessionid);
                }
                else if (input[0] == "logout")
                {
                    s.logout();
                }

            }

          
       
        }


        /////////////////////////////

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


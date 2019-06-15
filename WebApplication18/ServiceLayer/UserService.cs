using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using workshop192.Bridge;
using WebApplication18.Domain;
using System.Security.Cryptography;

namespace workshop192.ServiceLayer
{

    public class UserService

    {
        private static UserService instance;
        private DomainBridge db = DomainBridge.getInstance();
        public static UserService getInstance()
        {
            if (instance == null)
                instance = new UserService();
            return instance;
        }

        private UserService()
        {

        }

        public void testSetup()
        {
            db.testSetup();
        }
        public void setup()
        {
            db.setup();
        }

        // use case 2.1 - the constructor defines guest as the default state
        public int startSession()
        {
            return db.startSession();
        }

        //use case 2.3
        public void login(int session, String username, String password)
        {
            db.login(session, username, password);
        }
        //7
        public int payToExternal(string card, string month, string year, string holder, string ccv, string id)
        {
            return db.payToExternal( card,  month,  year,  holder,  ccv,  id);
        }

        //use case 2.2
        public void register(int user, String username, String password)
        {
            if (user < 0)
            {
                throw new NullReferenceException("error - bad session");
            }
            if (username.Equals("") || password.Equals(""))
            {
                throw new ArgumentException("Error: Illegal username or password");
            }
            String encrypted = encryptPassword(password);
            db.register(user, username, encrypted);
        }

        public int cancelPay(int result)
        {
            return db.cancelPay(result);
        }
        //8
        public int deliverToExternal(string name, string address, string city, string country, string zip, string cvv)
        {
            return db.deliverToExternal(name, address, city, country, zip, cvv);
        }

        public string encryptPassword(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }


        internal LinkedList<int> getSessionByUserName(string username)
        {
            return db.getSessionByUserName(username);
        }

        internal void setWaitingMessages(LinkedList<Tuple<string, string>> remains)
        {
            db.setWaitingMessages(remains);
        }

        //use case 6.2
        public void removeUser(int admin, String username)
        {
            if (admin < 0)
            {
                throw new NullReferenceException("error - bad session");
            }
            db.removeUser(admin, username);
        }

        public int cancelDelivery(int result)
        {
           return  db.cancelDelivery(result);
        }
        //3.1
        public void logout(int user)
        {
            if (user < 0)
            {
                throw new NullReferenceException("error - bad session");
            }
            db.logout(user);
        }
        //3.2
        public int createStore(int session, String storeName, String description)
        {
            if (session < 0)
            {
                throw new NullReferenceException("error - bad session");
            }
            return db.createStore(session, storeName, description);
        }


        public string getAllProducts()
        {
            return db.getAllProducts();
        }
        public string getAllStores(int session)
        {

            return db.getAllStores(session);
        }

        //use case 2.5

        public string searchByName(String name)
        {
            return db.searchProducts(name, null,null);
        }

        public string searchByKeyword(String keyword)
        {
            return db.searchProducts(null, keyword, null);
        }

        public string searchByCategory(String category)
        {
            return db.searchProducts(null, null, category);
        }

        
        public static string getStateName(string hash)
        {
            try
            {
                return DomainBridge.getInstance().getState(hash);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal LinkedList<string> getMessagesFor(string username)
        {
            return db.getMessagesFor(username);
        }

        internal void clearMessagesFor(string username)
        {
            db.clearMessagesFor(username);
        }
        public void checkBasket(int session)
        {
            if (session < 0)
            {
                throw new NullReferenceException("error - bad session");
            }
             db.checkBasket(session);
        }
        //2.7
        public void addToShoppingBasket(int product, int amount, int session)
        {
            if (product < 0)
            {
                throw new ArgumentException("invalid product id");
            }

            if (session < 0)
            {
                throw new ArgumentException("error - bad session");
            }
            db.addToShoppingBasket(product, amount, session);
        }

        public void removeFromShoppingBasket(int session, int productId)
        {
            if (productId < 0)
            {
                throw new ArgumentException("invalid product id");
            }


            db.removeFromCart(session, productId);
        }
        //use case 2.7
        public string getShoppingBasket(int session)
        {
            string jsonBasket = db.getShoppingBasket(session);
            return jsonBasket;
        }

        public void purchaseBasket(int session, string address, string creditcard, string month, string year, string  holder, string cvv)
        {
            db.purchaseBasket(session, address, creditcard, month, year, holder, cvv);
        }
        public bool handShake()
        {
           bool res= db.handShakePay();
            if (res == false)
                return false;
           return db.handShakeDeliver();
        }
       


        /////////////////////////////////////////////////////////////////////////////////////
        public void addUser(string hash, int session)
        {
            db.addSession(hash, session);
        }

        public string generate()
        {
            return db.generate();
        }
        
        public int getUserByHash(string hash)
        {
            return db.getUserByHash(hash);
            
        }

        public LinkedList<Tuple<string, string>> getWaitingMessages()
        {
            return db.getWaitingNotifications();
        }
        
        internal string getUserNameBySession(int sessionid)
        {
            return db.getUserNameBySession(sessionid);
        }

        internal void addWaitingMessage(Tuple<string, string> tuple)
        {
            db.addWaitingMessage(tuple);
        }

        public int getNumOfProductsInBasket(int session)
        {
            return db.getNumOfProductsInBasket(session);
        }
    }
}


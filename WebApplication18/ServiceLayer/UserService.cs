using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;
using workshop192.Bridge;
using WebApplication18.Domain;

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
        public void login(int user, String username, String password)
        {
            db.login(user, username, password);
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
                throw new ArgumentException("Illegal username or password");
            }
            db.register(user, username, password);
        }

        internal int getSessionByUserName(string username)
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

        public void logout(int user)
        {
            if (user < 0)
            {
                throw new NullReferenceException("error - bad session");
            }
            db.logout(user);
        }

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
        public LinkedList<Store> getAllStores(int session)
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

        public List<Product> filterProducts(List<Product> list, int[] price_range, int minimumRank)
        {
            return db.filterProducts(list, price_range, minimumRank);

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

        public string getShoppingBasket(int session)
        {
            string jsonBasket = db.getShoppingBasket(session);
            return jsonBasket;
        }

        public void purchaseBasket(int session)
        {
            db.purchaseBasket(session);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        public String addUser(string hash, int session)
        {
            return DBCookies.getInstance().addSession(hash, session);
        }

        public string generate()
        {
            return DBCookies.getInstance().generate();
        }
        
        public int getUserByHash(string hash)
        {
            return DBCookies.getInstance().getUserByHash(hash);
        }

        public LinkedList<Tuple<string, string>> getWaitingMessages()
        {
            return db.getWaitingNotifications();
        }

        internal void addWaitingMessage(Tuple<string, string> tuple)
        {
            db.getWaitingNotifications().AddFirst(tuple);
        }

        internal string getUserNameBySession(int sessionid)
        {
            return db.getUserNameBySession(sessionid);
        }
    }
}

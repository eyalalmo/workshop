using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;
using workshop192.Bridge;
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

        public void purchaseBasket(int session, string address, string creditCard)
        {
            db.purchaseBasket(session, address, creditCard);
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


        ///////////////////////////////////////////////////

        public void addProductVisibleDiscount(int product, int session, double percentage, string duration)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.addProductVisibleDiscount(product, percentage, duration, session);


        }

        public void removeProductDiscount(int product, int session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.removeProductDiscount(product, session);

        }


        public void setProductDiscount(int product, int discount, int session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            //if (product < 0)
            //    throw new ArgumentException("illegal discount number");
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.setProductDiscount(product, discount, session);
        }

        public void addStoreVisibleDiscount(int store, double percentage, string duration, int session)
        {
            if (store < 0)
                throw new ArgumentException("illegal store number");
            if (percentage <= 0 || percentage >= 1)
            {
                throw new IllegalAmountException("percentage of discount must be a number between 0 to 1");
            }
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.addStoreVisibleDiscount(store, percentage, duration, session);
        }

        public void addReliantDiscountSameProduct(int store, int session, double percentage, String duration, int numOfProducts, int product)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");

            if (store < 0)
                throw new ArgumentException("illegal store number");
            if (percentage <= 0 || percentage >= 1)
            {
                throw new IllegalAmountException("percentage of discount must be a number between 0 to 1");
            }
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.addReliantdiscountSameProduct(store, product, percentage, numOfProducts, duration, session);
        }
        public void addReliantDiscountTotalAmount(int store, int session, double percentage, String duration, int amount)
        {
            if (store < 0)
                throw new ArgumentException("illegal store number");
            if (percentage <= 0 || percentage >= 1)
            {
                throw new IllegalAmountException("percentage of discount must be a number between 0 to 1");
            }
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.addReliantdiscountTotalAmount(store, percentage, amount, duration, session);
        }

        public void removeStoreDiscount(int store, int sessionID)
        {
            if (store < 0)
                throw new ArgumentException("illegal store number");
            db.removeStoreDiscount(store, sessionID);
        }
        public void addComplexDiscount(List<DiscountComponent> list, string type)
        {
            throw new NotImplementedException();
        }

        public void addMinAmountPolicy(int storeID, int sessionID, int minAmount)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");
            
            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.addMinAmountPolicy(storeID, sessionID, minAmount);
        }
        public void setMinAmountPolicy(int storeID, int sessionID, int newMinAmount)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.setMinAmountPolicy(storeID, sessionID, newMinAmount);

        }
        public void removeMinAmountPolicy(int storeID, int sessionID)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.removeMaxAmountPolicy(storeID, sessionID);


        }

        public void addMaxAmountPolicy(int storeID, int sessionID, int maxAmount)
            {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.addMaxAmountPolicy(storeID, sessionID, maxAmount);
        }
        public void setMaxAmountPolicy(int storeID, int sessionID, int newMaxAmount)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.setMaxAmountPolicy(storeID, sessionID, newMaxAmount);

        }
        public void removeMaxAmountPolicy(int storeID, int sessionID)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.removeMaxAmountPolicy(storeID, sessionID);


        }


    }
}


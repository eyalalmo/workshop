using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;
using workshop192.Bridge;
namespace workshop192.ServiceLayer
{

   public  class UserService

    {
        private static UserService instance;
        private DomainBridge db= DomainBridge.getInstance();
        public static UserService getInstance()
        {
            if (instance == null)
                instance = new UserService();
            return instance;
        }

        private UserService()
        {

        }

        // use case 2.1 - the constructor defines guest as the default state
        public Session startSession()
        {
            return new Session();
        }

        //use case 2.3
        public void login(Session user, String username, String password)
        {
            db.login(user,username, password);
        }

        //use case 2.2
        public void register(Session user, String username, String password)
        {
            if (user == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            if (username.Equals("") || password.Equals(""))
            {
                throw new ArgumentException( "Illegal username or password");
            }
            db.register(user,username, password);
        }
        //use case 6.2
        public void removeUser(Session admin, String username)
        {
            if (admin == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            db.removeUser(admin,username);
        }

        public void logout(Session user)
        {
            if (user == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            db.logout(user);
        }

        public int createStore(Session session, String storeName, String description)
        {
            if (session == null)
            {
                return -1;
            }
            return db.createStore(session,storeName, description);
        }

        //use case 2.5

        public List<Product> searchProducts(String name, String keywords, String category)
        {
            return db.searchProducts(name, keywords, category);
        }

        public List<Product> filterProducts(List<Product> list, int[] price_range, int minimumRank)
        {
            return db.filterProducts(list, price_range, minimumRank);

        }

        public void addToShoppingBasket(int product, int amount, Session session)
        {
            if (product < 0)
            {
                throw new ArgumentException("invalid product id");
            }

            if (session == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            db.addToShoppingBasket(product, amount,session);
        }

        public void purchaseBasket(Session session)
        {
             db.purchaseBasket(session);
        }

        public int createSession()
        {
            throw new NotImplementedException();
        }
    }
}

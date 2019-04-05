using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.ServiceLayer
{

   public  class UserService

    {
        private static UserService instance;

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
        public String login(Session user, String username, String password)
        {
            if (user == null)
            {
                return "ERROR: bad session";
            }
            return user.login(username, password);
        }

        //use case 2.2
        public String register(Session user, String username, String password)
        {
            if (user == null)
            {
                return "ERROR: bad session";
            }
            if (username.Equals("") || password.Equals(""))
            {
                return "Illegal username or password";
            }
            return user.register(username, password);
        }
        //use case 6.2
        public String removeUser(Session admin, String username)
        {
            if (admin == null)
            {
                return "ERROR: bad session";
            }
            return admin.removeUser(username);
        }

        public String logout(Session user)
        {
            if (user == null)
            {
                return "ERROR: bad session";
            }
            return user.
                
               logout();
        }

        public Store createStore(Session session, String storeName, String description)
        {
            if (session == null)
            {
                return null;
            }
            return session.createStore(storeName, description);
        }

        //use case 2.5

        public List<Product> searchProducts(String name, String keywords, String category)
        {
            return DBProduct.getInstance().searchProducts(name, keywords, category);
        }

        public List<Product> filterProducts(List<Product> list, int[] price_range, int minimumRank)
        {
            return DBProduct.getInstance().filterBy(list, price_range, minimumRank);

        }

        public String addToShoppingBasket(Product product, int amount, Session session)
        {
            if (session == null)
            {
                return "ERROR: session is null";
            }
            return session.addToShoppingBasket(product, amount);
        }

        public String purchaseBasket(Session session)
        {
            return session.purchaseBasket();
        }






    }
}

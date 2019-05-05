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
        public void login(Session user, String username, String password)
        {
            if (user == null)
            {
                throw new NullReferenceException("error - no such user ID");
            }
             user.login(username, password);
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
            user.register(username, password);
        }
        //use case 6.2
        public void removeUser(Session admin, String username)
        {
            if (admin == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            admin.removeUser(username);
        }

        public void logout(Session user)
        {
            if (user == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            user.logout();
        }

        public Store createStore(Session session, String storeName, String description)
        {
            if (session == null)
            {
                throw new NullReferenceException("error - bad session");
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

        /*public void addToShoppingBasket(Product product, int amount, Session session)
        {
            if (session == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            session.addToShoppingBasket(product, amount);
        }*/

        public void purchaseBasket(Session session)
        {
             session.purchaseBasket();
        }

        /////////////////////////////////////////////////////////////////////////////////////
        public static String addUser(string hash, Session session)
        {
            return DBCoockies.getInstance().addSession(hash, session);
        }

        public static string generate()
        {
            return DBCoockies.getInstance().generate();

        }


        public static Session getUserByHash(string hash)
        {
            return DBCoockies.getInstance().getUserByHash(hash);
        }

        public static Session getHashByName(String name)
        {
            return DBCoockies.getInstance().getUserByName(name);
        }

        ////////////////////////






    }
}

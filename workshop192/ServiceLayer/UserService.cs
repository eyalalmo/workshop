using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.ServiceLayer
{
    class UserService
    {
        private static UserService instance;

        public static UserService getInstance() {
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
            return user.logout();
        }

        /*
        public String getPurchaseHistory(Session user)
        {
            if (user == null)
            {
                return "ERROR: bad session";
            }
            return user.getPurchaseHistory();
        }
        

        //use case 2.4
        /*public LinkedList<Product> getStoreProducts(int storeID) 
        {

            Store s = DBStore.getInstance().getStore(storeID);

            if (s == null)
            {
                return null;
            }
            return s.getProductList();
        }
        //use case 2.4
        public LinkedList<Product> getAllProducts()
        {
            return DBProduct.getInstance().getAllProducts();
        }*/
        //use case 2.5
        public List<Product> searchProducts(String name, String keywords, String category, int[] price_range,int minimumRank)
        {
            return DBProduct.getInstance().searchProducts(name, keywords, category, price_range,minimumRank);
        }


       


    }
}

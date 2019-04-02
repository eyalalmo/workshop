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
        public UserService() { }

        public String login(Session user, String username, String password)
        {
            if (user == null)
            {
                return "ERROR: bad session";
            }
            return user.login(username, password);
        }
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


        public String getPurchaseHistory(Session user)
        {
            if (user == null)
            {
                return "ERROR: bad session";
            }
            return user.getPurchaseHistory();
        }


        public LinkedList<Product> getStoreProducts(int storeID) 
        {

            Store s = DBStore.getInstance().getStore(storeID);

            if (s == null)
            {
                return null;
            }
            return s.getProductList();
        }

        public LinkedList<Product> getAllProducts()
        {
            return DBProduct.getInstance().getAllProducts();
        }

        public LinkedList<Product> searchProducts(String name, String keywords, String category, int[] price_range,int minimumRank)
        {
            return DBProduct.searchProducts(name, keywords, category, price_range,minimumRank);
        }


       


    }
}

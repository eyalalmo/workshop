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

        public String login(User user, String username, String password)
        {
            
            return user.login(username, password);
        }
        public String register(User user, String username, String password)
        {
            if (username.Equals("") || password.Equals(""))
            {
                return "Illegal username or password";
            }
            return user.register(username, password);
        }

        public String deleteUser(User admin, String username)
        {
           return admin.deleteUser(username);
        }

        public String logout(User user)
        {
            return user.getState().logout();
        }

        public String closeStore(User user, int storeID)
        {
            return user.getState().closeStore(storeID);
        }

        public String getPurchaseHistory(User user)
        {
            return user.getState().getPurchaseHistory();
        }

        public String removeUser(User admin,String username)
        {
            return admin.removeUser(username);
        }

        public LinkedList<Product> getStoreProducts(int storeID) 
        {
            Store s = DB.getInstance().getStore(storeID);
            if (s == null)
            {
                return null;
            }
            return s.showProducts();
        }

        public LinkedList<Product> getAllProducts()
        {
            return DB.getAllProducts();
        }


    }
}

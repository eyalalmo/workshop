using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{

    class Admin : UserState
    {
        public string closeStore(int id)
        {
            Store store = DBStore.getStore(id);
            if (store == null)
            {
                return "ERROR: store does not exist";
            }
            else
            {
                return DB.removeStore(store);
            }
        }

        public String createStore(String storeName, String description)
        {
            Store store = new Store(storeName, description);
            return DBStore.add(store);
        }

        public String getPurchaseHistory(SubscribedUser sub)
        {
            throw new NotImplementedException();
        }

        public String login(string username, string password, Session session)
        {
            return "ERROR: User already logged in";
        }

        public string logout(SubscribedUser sub)
        {
            return DBSubscribedUser.logout(sub);
        }

        public string register(string username, string password)
        {
            return "ERROR: User already registered";
        }

        public string removeUser(string username)
        {
            throw new NotImplementedException();
        }


    }
}

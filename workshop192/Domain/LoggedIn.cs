using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class LoggedIn : UserState
    {
        private DBSubscribedUser dbSubscribedUser;

        public LoggedIn()
        {
            dbSubscribedUser = DBSubscribedUser.getInstance();
        }

        public string closeStore(int id)
        {
            return "ERROR: not an admin";
        }

        public string createStore(String storeName, String description)
        {
            return "ERROR: not an admin";
        }

        public string getPurchaseHistory(SubscribedUser sub)
        {
<<<<<<< HEAD

            return sub.getPurchaseHistory();
=======
            return DBSubscribedUser.getInstance().getPurchaseHistory(sub);
            return DBSubscribedUser.getPurchaseHistory(sub);
            return dbSubscribedUser.getPurchaseHistory(sub);
>>>>>>> origin/Stores_and_Products
        }

        public string login(string username, string password, Session session)
        {
            return "ERROR: User already logged in";
        }

        public string logout(SubscribedUser sub, Session session)
        {

<<<<<<< HEAD
=======
            return DBSubscribedUser.getInstance().logout(sub);
            return DBSubscribedUser.logout(sub);
>>>>>>> origin/Stores_and_Products

            String logoutResponse = dbSubscribedUser.logout(sub);
            if (Equals(logoutResponse, ""))
            {
                session.setState(new Guest());
            }
            return logoutResponse;
<<<<<<< HEAD

=======
>>>>>>> origin/Stores_and_Products
        }

        public string register(string username, string password, Session session)
        {
            return "ERROR: User already registered";
        }

        public string removeUser(string username)
        {
            return "ERROR: not an admin";
        }
    }
}

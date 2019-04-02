using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class LoggedIn : UserState
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

        public string createStore(int id, String storeName, String description)
        {
            return "ERROR: not an admin";
        }

        public string getPurchaseHistory(SubscribedUser sub)
        {

            return sub.getPurchaseHistory();
        }

        public string login(string username, string password, Session session)
        {
            return "ERROR: User already logged in";
        }

        public string logout(SubscribedUser sub, Session session)
        {
            String logoutResponse = dbSubscribedUser.logout(sub);
            if (Equals(logoutResponse, ""))
            {
                session.setState(new Guest());
            }
            return logoutResponse;

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

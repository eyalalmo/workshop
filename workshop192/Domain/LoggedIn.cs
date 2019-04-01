using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class LoggedIn : UserState
    {
        public string closeStore(int id)
        {
            return "ERROR: not an admin";
        }

        public string createStore()
        {
            return "ERROR: not an admin";
        }

        public string getPurchaseHistory()
        {
            return DBSubscribedUser.getPurchaseHistory();
        }

        public string login(string username, string password, ref SubscribedUser subscribedUser)
        {
            return "ERROR: User already logged in";
        }

        public string logout()
        {
            return DBSubscribedUser.logout();
        }

        public string register(string username, string password)
        {
            return "ERROR: User already registered";
        }

        public string removeUser(string username)
        {
            return "ERROR: not an admin";
        }
    }
}

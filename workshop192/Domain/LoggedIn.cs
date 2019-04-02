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

        public string getPurchaseHistory(SubscribedUser sub)
        {
<<<<<<< HEAD
            return DBSubscribedUser.getInstance().getPurchaseHistory(sub);
=======
            return DBSubscribedUser.getPurchaseHistory(sub);
>>>>>>> origin/etay_v3
        }

        public string login(string username, string password, Session session)
        {
            return "ERROR: User already logged in";
        }

        public string logout(SubscribedUser sub)
        {
<<<<<<< HEAD
            return DBSubscribedUser.getInstance().logout(sub);
=======
            return DBSubscribedUser.logout(sub);
>>>>>>> origin/etay_v3
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

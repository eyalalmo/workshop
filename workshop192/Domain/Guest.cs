using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;


namespace workshop192.Domain
{
    class Guest : UserState
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
            return "ERROR: not an admin";
        }

        public String login(String username, String password, ref SubscribedUser subscribedUser)
        {
            SubscribedUser sub = DBSubscribedUser.getInstance().getSubscribedUser(username);

            if (sub != null)
            {
                if (Equals(sub.getPassword(), password))
                {
                    subscribedUser = sub;
                    return DBSubscribedUser.getInstance().login(subscribedUser);
                }
                else
                {
                    return "ERROR: password incorrect";
                }
            }
            else
            {
                return "ERROR: username does not exist";
            }
        }

        public string logout(SubscribedUser sub)
        {
            return "ERROR: not logged in";
        }

        public string register(string username, string password, Session user)
        {
            if (user != null)
            {
                return "ERROR: username already exists";
            }
            else
            {
                SubscribedUser sub = new SubscribedUser(username, password);
                return DBSubscribedUser.getInstance().register(sub);
            }
        }

        public string removeUser(string username)
        {
            return "ERROR: not an admin";
        }
    }
}

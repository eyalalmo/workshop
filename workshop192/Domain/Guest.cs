using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using workshop192.Domain;


namespace workshop192.Domain
{
    public class Guest : UserState
    {
        private DBSubscribedUser dbSubscribedUser;

        public Guest()
        {
            dbSubscribedUser = DBSubscribedUser.getInstance();
        }

        public string closeStore(Store store)
        {
            return "ERROR: not an admin";
        }

        public string complain(string description, SubscribedUser subscribedUser)
        {
            return "ERROR: guest cannot complain";
        }

        public Store createStore(String storeName, String description, SubscribedUser sub)
        {
            return null;
        }

        public String getComplaints()
        {
            return "ERROR: only an admin can getComplaints";
        }

        public string getPurchaseHistory(SubscribedUser sub)
        {
            return "ERROR: not a subscribed user";
        }

        public String login(String username, String password, Session session)
        {
            String encrypted = DBSubscribedUser.getInstance().encryptPassword(password);
            SubscribedUser sub = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (sub == null)
                return "ERROR: username does not exist";
            if(!Equals(sub.getPassword(), encrypted))
                return "ERROR: password incorrect";
            session.setSubscribedUser(sub);
            if (Equals(username, "admin"))
            {
                session.setState(new Admin());

            }
            else
            {
                session.setState(new LoggedIn());
            }
            session.setShoppingBasket(sub.getShoppingBasket());
            return DBSubscribedUser.getInstance().login(sub);
        }

        public string logout(SubscribedUser sub, Session session)
        {
            return "ERROR: not logged in";
        }

        public string register(string username, string password, Session session)
        {
            String encrypted = DBSubscribedUser.getInstance().encryptPassword(password);
            SubscribedUser s = dbSubscribedUser.getSubscribedUser(username);
            if (s != null)
                return "ERROR: username already exists";
            SubscribedUser sub = new SubscribedUser(username, encrypted, session.getShoppingBasket());
            return DBSubscribedUser.getInstance().register(sub);
        }

        public string removeUser(String user)
        {
            return "ERROR: not an admin";
        }
    }
}

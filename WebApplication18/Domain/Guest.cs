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


        public void closeStore(Store store)
        {
            throw new UserStateException("Guest cannot close a store");
        }

        public void complain(string description, SubscribedUser subscribedUser)
        {
            throw new UserStateException("Guest cannot complain");
        }

        public Store createStore(String storeName, String description, SubscribedUser sub)
        {
            throw new UserStateException("Guest cannot create a store");
        }

        public String getComplaints()
        {
            throw new UserStateException("Guest cannot get complaints");
        }

        public string getPurchaseHistory(SubscribedUser sub)
        {
            throw new UserStateException("Guest does not have a purchase history");
        }

        public void login(String username, String password, Session session)
        {
            String encrypted = DBSubscribedUser.getInstance().encryptPassword(password);
            SubscribedUser sub = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (sub == null)
                throw new LoginException("Username does not exist");
            SubscribedUser loggedIn = DBSubscribedUser.getInstance().getloggedInUser(username);
            if( loggedIn != null)
                throw new LoginException("Username already logged in");
            if (!Equals(sub.getPassword(), encrypted))
                throw new LoginException("Incorrect password");
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
            DBSubscribedUser.getInstance().login(sub);
        }

        public void logout(SubscribedUser sub, Session session)
        {
            throw new UserStateException("Guest cannot logout");
        }

        public void register(string username, string password, Session session)
        {
            String encrypted = DBSubscribedUser.getInstance().encryptPassword(password);
            SubscribedUser s = dbSubscribedUser.getSubscribedUser(username);
            if (s != null)
               throw new RegisterException("username already exists");
            SubscribedUser sub = new SubscribedUser(username, encrypted, session.getShoppingBasket());
            session.setSubscribedUser(sub);
            DBSubscribedUser.getInstance().register(sub);
        }

        public void removeUser(String user)
        {
            throw new UserStateException("ERROR: not an admin");
        }
    }
}

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
            throw new UserStateException("Error: A Guest cannot close a store");
        }

        public void complain(string description, SubscribedUser subscribedUser)
        {
            throw new UserStateException("Error: A Guest cannot complain");
        }

        public Store createStore(String storeName, String description, SubscribedUser sub)
        {
            throw new UserStateException("Error: A Guest cannot create a store");
        }

        public String getComplaints()
        {
            throw new UserStateException("Error: A Guest cannot get complaints");
        }

        public string getPurchaseHistory(SubscribedUser sub)
        {
            throw new UserStateException("Error: A Guest does not have a purchase history");
        }

        public string getStateName()
        {
            return "Guest";
        }

        public void login(String username, String password, Session session)
        {
            String encrypted = DBSubscribedUser.getInstance().encryptPassword(password);
            SubscribedUser sub = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (sub == null)
                throw new LoginException("Error: Username does not exist");
            DBSubscribedUser.getInstance().updateStoreRole(sub);
            SubscribedUser loggedIn = DBSubscribedUser.getInstance().getloggedInUser(username);
            if( loggedIn != null)
                throw new LoginException("Error: Username already logged in");
            if (!Equals(sub.getPassword(), encrypted))
                throw new LoginException("Error: Incorrect password");
            ////////////erase

           // Store st = new Store("bb", "cc");
            //DBStore.getInstance().addStore(st);
            


            ////////erase
            session.setSubscribedUser(sub);
            
            if (Equals(username, "u1"))
            {
                session.setState(new Admin());
            }
            else
            {
                session.setState(new LoggedIn());
            }
            session.setShoppingBasket(new ShoppingBasket(sub.getUsername()));
            session.setShoppingBasket(sub.getShoppingBasket());
            DBSubscribedUser.getInstance().login(sub);
        }

        public void logout(SubscribedUser sub, Session session)
        {
            throw new UserStateException("Error: You're not logged in");
        }

        public void register(string username, string password, Session session)
        {
            SubscribedUser s = dbSubscribedUser.getSubscribedUser(username);
            if (s != null)
               throw new RegisterException("Error: Username already exists");
            session.getShoppingBasket().setUsername(username);
            SubscribedUser sub = new SubscribedUser(username, encrypted, session.getShoppingBasket());

            //session.setSubscribedUser(sub);
            DBSubscribedUser.getInstance().register(sub);
        }
        
        public void removeUser(string subscribedUser)
        {
            throw new NotImplementedException();
        }
    }
}

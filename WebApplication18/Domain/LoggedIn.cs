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
        private DBComplaint dbComplaint;

        public LoggedIn()
        {
            dbSubscribedUser = DBSubscribedUser.getInstance();
            dbComplaint = DBComplaint.getInstance();
        }

        public void closeStore(Store store)
        {
            throw new UserStateException("Error: Only an Admin can close stores");
        }

        public void complain(string description, SubscribedUser subscribedUser)
        {
            Complaint complaint = new Complaint(subscribedUser.getUsername(), description);
            dbComplaint.addComplaint(complaint);
        }

        public Store createStore(String storeName, String description, SubscribedUser sub)
        {
            if (storeName == "")
            {
                throw new IllegalNameException();
            }
            Store store = new Store(storeName, description);
            StoreOwner owner = new StoreOwner(null, sub, store);
            store.addStoreRole(owner);
            sub.addStoreRole(owner);
            DBStore.getInstance().addStore(store);
            DBStore.getInstance().addStoreRole(owner);
            return store;
        }

        public String getComplaints()
        {
            throw new UserStateException("Error: Only an Admin can get complaints");
        }

        public string getPurchaseHistory(SubscribedUser sub)
        {
            return sub.getPurchaseHistory();
        }

        public string getStateName()
        {
            return "LoggedIn";
        }

        public void login(string username, string password, Session session)
        {
            throw new LoginException("Error: User is already logged in");
        }

        public void logout(SubscribedUser sub, Session session)
        {
            dbSubscribedUser.logout(sub);
            session.setState(new Guest());
            session.setShoppingBasket(new ShoppingBasket());

        }

        public void register(string username, string password, Session session)
        {
            throw new UserStateException("Error: User already registered");
        }

        public void removeUser(String user)
        {
            throw new UserStateException("Error: Not an admin");
        }
    }
}

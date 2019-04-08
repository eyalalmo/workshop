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

        public string addToShoppingBasket(Product product, int amount, ShoppingBasket basket)
        {
            return basket.addToCart(product, amount);
        }

        public void closeStore(Store store)
        {
            throw new UserStateException("Logged In user cannot close a store");
        }

        public void complain(string description, SubscribedUser subscribedUser)
        {
            Complaint complaint = new Complaint(subscribedUser.getUsername(), description);
            dbComplaint.addComplaint(complaint);
        }

        public Store createStore(String storeName, String description, SubscribedUser sub)
        {
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
            throw new UserStateException("Logged in user cannot get complaints");
        }

        public string getPurchaseHistory(SubscribedUser sub)
        {
            return sub.getPurchaseHistory();
        }

        public string login(string username, string password, Session session)
        {
            throw new LoginException("User already logged in");
        }

        public void logout(SubscribedUser sub, Session session)
        {
            dbSubscribedUser.logout(sub);
            session.setState(new Guest());
        }

        public string purchaseBasket(ShoppingBasket basket)
        {
            return basket.purchaseBasket();
        }

        public void register(string username, string password, Session session)
        {
            throw new UserStateException("ERROR: User already registered");
        }

        public void removeUser(String user)
        {
            throw new UserStateException("ERROR: not an admin");
        }
    }
}

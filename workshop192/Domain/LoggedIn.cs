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

        public string closeStore(Store store)
        {
            return "ERROR: not an admin";
        }

        public string complain(string description, SubscribedUser subscribedUser)
        {
            Complaint complaint = new Complaint(subscribedUser.getUsername(), description);
            return dbComplaint.addComplaint(complaint);
        }

        public Store createStore(String storeName, String description, SubscribedUser sub)
        {
            Store store = new Store(storeName, description);
            StoreOwner owner = new StoreOwner(null, sub, store);
            store.addStoreRole(owner);
            sub.addStoreRole(owner);
            DBStore.getInstance().addStore(store);
            return store;
        }

        public string getComplaints()
        {
            return "ERROR: only an admin can getComplaints";
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

        public string removeUser(String user)
        {
            return "ERROR: not an admin";
        }
    }
}
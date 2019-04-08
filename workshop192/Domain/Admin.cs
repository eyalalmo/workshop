using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{

    public class Admin : UserState
    {
        private DBStore dbStore;
        private DBSubscribedUser dbSubscribedUser;
        private DBSession dbSession;
        private DBComplaint dbComplaint;

        public Admin()
        {
            dbStore = DBStore.getInstance();
            dbSubscribedUser = DBSubscribedUser.getInstance();
            dbSession = DBSession.getInstance();
            dbComplaint = DBComplaint.getInstance();
        }

        public string addToShoppingBasket(Product product, int amount, ShoppingBasket basket)
        {
            return "ERROR: admin cannot add to shopping basket";
        }

        public string closeStore(Store store)
        {
            List<StoreRole> roles = store.getRoles();
            foreach (StoreRole role in roles)
            {
                SubscribedUser sub = role.getUser();
                sub.removeStoreRole(role);
            }
            return dbStore.removeStore(store);

        }

        public string complain(string description, SubscribedUser subscribedUser)
        {
            return "ERROR: admin cannot complain";
        }

        public Store createStore(String storeName, String description, SubscribedUser sub)
        {
            return null;
        }

        public String getComplaints()
        {
            LinkedList<Complaint> complaints = dbComplaint.getComplaints();
            String str = "";
            foreach (Complaint c in complaints)
            {
                str = str + c.toString();
            }
            return str;
        }


        public String getPurchaseHistory(SubscribedUser sub)
        {
            return sub.getPurchaseHistory();
        }

        public void login(string username, string password, Session session)
        {
            throw new LoginException("Admin already logged in");
        }

        public void logout(SubscribedUser sub, Session session)
        {
            dbSubscribedUser.logout(sub);
            session.setState(new Guest());
        }

        public string purchaseBasket(ShoppingBasket basket)
        {
            return "ERROR: admin cannot make a purchase";
        }

        public String register(string username, string password, Session session)
        {
            throw new RegisterException("Admin is already logged in, cannot register");
        }

        public void removeUser(String user)
        {
            if (Equals(user, "admin"))
                throw new UserException("admin cannot be removed");
            SubscribedUser subscribedUser = DBSubscribedUser.getInstance().getSubscribedUser(user);
            if (subscribedUser == null)
                throw new UserException("user to be removed does not exist");
            Session session = dbSession.getSessionOfSubscribedUser(subscribedUser);
            if (session != null)
            {
                if (session.getState() is LoggedIn)
                {
                    session.logout();
                }
            }
            foreach (StoreRole role in subscribedUser.getStoreRoles())
            {
                role.removeAllAppointedBy();
                Store store = role.getStore();
                SubscribedUser appointedBySubscribedUser = role.getAppointedBy();
                if (appointedBySubscribedUser != null)
                {
                    StoreRole appointedByStoreRole = store.getStoreRole(role.getAppointedBy());
                    store.removeStoreRole(appointedByStoreRole);
                    appointedByStoreRole.removeRoleAppointedByMe(role);
                }
                if (role is StoreOwner && role.getStore().getNumberOfOwners() == 0)
                {
                    closeStore(role.getStore());
                }
                DBStore.getInstance().removeStoreRole(role);

            }
            session.setSubscribedUser(null);
            dbSubscribedUser.remove(subscribedUser);

        }


    }

}

using System;
using System.Collections.Generic;
using System.Linq;
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
        public string closeStore(Store store)
        {
            List<StoreRole> roles = store.getRoles();
            foreach(StoreRole role in roles)
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

        public String createStore(String storeName, String description, SubscribedUser sub)
        {
            return "ERROR: admin cannot open a store";
        }

        public String getComplaints()
        {
            LinkedList<Complaint> complaints = dbComplaint.getComplaints();
            String str = "";
            foreach(Complaint c in complaints)
            {
                str = str + c.toString();
            }
            return str;
        }


        public String getPurchaseHistory(SubscribedUser sub)
        {
            return "ERROR: No purchase history in Admin";
        }

        public String login(string username, string password, Session session)
        {
            return "ERROR: User already logged in";
        }

        public String logout(SubscribedUser sub, Session session)
        {
            String logoutResponse = dbSubscribedUser.logout(sub);
            if (Equals(logoutResponse,""))
            {
                session.setState(new Guest());
            }
            return logoutResponse;
        }

        public String register(string username, string password, Session session)
        {
            return "ERROR: User already registered";
        }

        public String removeUser(SubscribedUser subscribedUser)
        {
            Session session = dbSession.getSessionOfSubscribedUser(subscribedUser);
            if (session != null)
            {
                if (session.getState() is LoggedIn)
                {
                    String logoutResponse = session.logout();
                    if (!Equals(logoutResponse, ""))
                        return logoutResponse;

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
                    appointedByStoreRole.remove(subscribedUser);
                }
                if (role is StoreOwner && role.getStore().getNumberOfOwners() == 1)
                {
                    closeStore(role.getStore());
                }
                else
                {
                    role.getStore().removeStoreRole(role);
                }
            }

            return dbSubscribedUser.remove(subscribedUser);

        }


    }
}

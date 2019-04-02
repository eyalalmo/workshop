using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{

    class Admin : UserState
    {
        private DBStore dbStore;
        private DBSubscribedUser dbSubscribedUser;
        private DBSession dbSession;

        public Admin()
        {
            dbStore = DBStore.getInstance();
            dbSubscribedUser = DBSubscribedUser.getInstance();
            dbSession = DBSession.getInstance();
        }
        public string closeStore(int id)
        {
            Store store = dbStore.getStore(id);
            if (store == null)
            {
                return "ERROR: store does not exist";
            }
            else
            {
                return dbStore.removeStore(store);
            }
        }

        public String createStore(int id, String storeName, String description)
        {
            Store store = new Store(id, storeName, description);
            dbStore.addStore(store);
            return "";
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

        public String removeUser(string username)
        {
            SubscribedUser sub = dbSubscribedUser.getSubscribedUser(username);
            if (sub == null)
                return "ERROR: user does not exist";
            Session session = dbSession.getSessionOfSubscribedUser(sub);
            if (session == null)
                return "ERROR: session does not exist";
            if (session.getState() is LoggedIn)
            {
                String logoutResponse = session.logout();
                if (!Equals(logoutResponse, ""))
                    return logoutResponse;

            }
            return dbStore.removeStoreByUser(sub);
        }


    }
}

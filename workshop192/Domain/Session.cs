using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class Session
    {
        private SubscribedUser subscribedUser;
        private UserState userState;

        public Session()
        {
            subscribedUser = null;
            userState = new Guest();
        }

        public String login(String username, String password)
        {
            return userState.login(username, password, out subscribedUser);
        }
  
        public String register(String username, String password)
        {
            return userState.register(username, password);
        }
        public String logout()
        {
            return userState.logout();
        }
        public String getPurchaseHistory()
        {
            return userState.getPurchaseHistory(subscribedUser);
        }
        public String createStore()
        {
            return userState.createStore();
        }
        public String closeStore(int id)
        {
            return userState.closeStore(id);
        }
        public String removeUser(String username)
        {
            return userState.removeUser(username);
        }


    }
}

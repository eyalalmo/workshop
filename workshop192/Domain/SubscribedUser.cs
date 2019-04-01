using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class SubscribedUser
    {
        private String username;
        private String password;

        public SubscribedUser(String username, String password)
        {
            this.username = username;
            this.password = password;
        }

        public String getPassword()
        {
            return this.password;
        }

        public String getUsername()
        {
            return this.username;
        }
        String login(String username, String password);
        String register(String username, String password);
        String logout();
        String getPurchaseHistory();
        String createStore();
        String closeStore(int id);
        String removeUser(String username);
        String getPassword() { return "" };
        String setPassword(String password);
        UserState getState();
    }
}

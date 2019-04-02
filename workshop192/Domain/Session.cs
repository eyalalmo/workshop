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
        private ShoppingBasket shoppingBasket;

        public Session()
        {
            subscribedUser = null;
            userState = new Guest();
            shoppingBasket = new ShoppingBasket();
        }

        public UserState getState()
        {
            return this.userState;
        }

        public SubscribedUser getSubscribedUser()
        {
            return this.subscribedUser;
        }

        public ShoppingBasket getShoppingBasket()
        {
            return this.shoppingBasket;
        }

        public void setSubscribedUser(SubscribedUser sub)
        {
            this.subscribedUser = sub;
        }

        public void setState(UserState state)
        {
            this.userState = state;
        }

        public String login(String username, String password)
        {
            return userState.login(username, password, this);
        }
  
        public String register(String username, String password)
        {
            return userState.register(username, password);
        }

        public String logout()
        {
            return userState.logout(subscribedUser);
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

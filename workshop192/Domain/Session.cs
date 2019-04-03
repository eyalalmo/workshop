using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class Session
    {
        private SubscribedUser subscribedUser;
        private UserState userState;
        private ShoppingBasket shoppingBasket;

        public Session()
        {
            subscribedUser = null;
            userState = new Guest();
            shoppingBasket = new ShoppingBasket();
            DBSession.getInstance().addSession(this);
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

        public void setShoppingBasket(ShoppingBasket shoppingB)
        {
            this.shoppingBasket = shoppingB;
        }

        public String login(String username, String password)
        {
            return userState.login(username, password, this);
        }
  
        public String register(String username, String password)
        {
            return userState.register(username, password, this);
        }

        public String logout()
        {
            return userState.logout(subscribedUser,this);
        }

        public String getPurchaseHistory()
        {
            return userState.getPurchaseHistory(subscribedUser);
        }

        public String createStore(int id, String storeName, String description)
        {
            return userState.createStore(id, storeName, description);
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

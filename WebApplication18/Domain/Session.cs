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
        private int id;

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

        public void setShoppingBasket(ShoppingBasket shoppingB)
        {
            this.shoppingBasket = shoppingB;
        }

        public void login(String username, String password)
        {
            userState.login(username, password, this);
        }

        public void loginAfterRegister(String username, String password)
        {
            userState.loginAfterRegister(username, password, this);
        }

        public void register(String username, String password)
        {
            userState.register(username, password, this);
        }

        public void logout()
        {
            userState.logout(subscribedUser, this);
        }

        public String getPurchaseHistory()
        {
            return userState.getPurchaseHistory(subscribedUser);
        }

        public Store createStore(String storeName, String description)
        {
            return userState.createStore(storeName, description, subscribedUser);
        }

        public void closeStore(Store store)
        {
            userState.closeStore(store);
        }

        public void removeUser(String user)
        {
            userState.removeUser(user);
        }

        public void complain(String description, SubscribedUser subscribedUser)
        {
            userState.complain(description, subscribedUser);
        }

        public void addToShoppingBasket(Product product, int amount)
        {
            if (amount < 0)
            {
                throw new AlreadyExistException("ERROR: amount cannot not be a negative number");
            }
            shoppingBasket.addToCart(product, amount);
        }
        public void removeFromCart(int productId)
        {
            shoppingBasket.removeFromCart(productId);
        }

        public  void purchaseBasket(string address, string creditcard, string month, string year, string holder, string cvv)
        
        {
           
                shoppingBasket.purchaseBasket(address, creditcard, month, year, holder, cvv);
        
            shoppingBasket = new ShoppingBasket();
        }
        public void checkBasket()
        {
            shoppingBasket.checkBasket();
        }


    }
}

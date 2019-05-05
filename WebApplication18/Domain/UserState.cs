using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public interface UserState
    {

        String login(String username, String password, Session session);
        String register(String username, String password, Session session);
        String logout(SubscribedUser sub, Session session);
        String getPurchaseHistory(SubscribedUser sub);
        Store createStore(String storeName, String description, SubscribedUser sub);
        String closeStore(Store store);
        String removeUser(String subscribedUser);
        String complain(String description, SubscribedUser subscribedUser);
        String getComplaints();
        String addToShoppingBasket(Product product, int amount, ShoppingBasket basket);
        String purchaseBasket(ShoppingBasket basket);
        String getStateName();
        

    }
}


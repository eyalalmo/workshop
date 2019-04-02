using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    interface UserState
    {
<<<<<<< HEAD

=======
>>>>>>> origin/Stores_and_Products
        String login(String username, String password, Session session);
        String register(String username, String password, Session session);
        String logout(SubscribedUser sub, Session session);
        String getPurchaseHistory(SubscribedUser sub);
        String createStore(String storeName, String description);
        String closeStore(int id);
        String removeUser(String username);
<<<<<<< HEAD

=======
        
>>>>>>> origin/Stores_and_Products
    }
}

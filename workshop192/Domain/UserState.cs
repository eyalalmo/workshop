using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    interface UserState
    {
        String login(String username, String password, Session session);
        String register(String username, String password);
        String logout(SubscribedUser sub);
        String getPurchaseHistory(SubscribedUser sub);
        String createStore();
        String closeStore(int id);
        String removeUser(String username);

    }
}

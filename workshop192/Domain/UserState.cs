using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    interface UserState
    {
        String login(String username, String password, ref UserState susbscribedUser);
        String register(String username, String password);
        String logout();
        String getPurchaseHistory();
        String createStore();
        String closeStore(int id);
        String removeUser(String username);

    }
}

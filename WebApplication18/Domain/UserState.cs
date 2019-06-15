using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace workshop192.Domain
{
    public interface UserState
    {
        void login(String username, String password, Session session);
        void loginAfterRegister(String username, String password, Session session);
        void register(String username, String password, Session session);
        void logout(SubscribedUser sub, Session session);
        String getPurchaseHistory(SubscribedUser sub);
        Store createStore(String storeName, String description, SubscribedUser sub);
        void closeStore(Store store);
        void removeUser(String subscribedUser);
        void complain(String description, SubscribedUser subscribedUser);
        String getComplaints();
        String getStateName();
    }
}


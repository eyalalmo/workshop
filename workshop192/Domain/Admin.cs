using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class Admin : UserState
    {
        public string closeStore(int id)
        {
            throw new NotImplementedException();
        }

        public string createStore()
        {
            throw new NotImplementedException();
        }

        public string getPurchaseHistory(SubscribedUser sub)
        {
            throw new NotImplementedException();
        }

        public string login(string username, string password, ref SubscribedUser subscribedUser)
        {
            throw new NotImplementedException();
        }

        public string logout(SubscribedUser sub)
        {
            throw new NotImplementedException();
        }

        public string register(string username, string password)
        {
            throw new NotImplementedException();
        }

        public string removeUser(string username)
        {
            throw new NotImplementedException();
        }
    }
}

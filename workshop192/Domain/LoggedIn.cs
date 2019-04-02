﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class LoggedIn : UserState
    {
        private DBSubscribedUser dbSubscribedUser;

        public LoggedIn()
        {
            dbSubscribedUser = dbSubscribedUser.getInstance();
        }

        public string closeStore(int id)
        {
            return "ERROR: not an admin";
        }

        public string createStore(String storeName, String description)
        {
            return "ERROR: not an admin";
        }

        public string getPurchaseHistory(SubscribedUser sub)
        {
            return dbSubscribedUser.getPurchaseHistory(sub);
        }

        public string login(string username, string password, Session session)
        {
            return "ERROR: User already logged in";
        }

        public string logout(SubscribedUser sub)
        {
            return dbSubscribedUser.logout(sub);
        }

        public string register(string username, string password, Session session)
        {
            return "ERROR: User already registered";
        }

        public string removeUser(string username)
        {
            return "ERROR: not an admin";
        }
    }
}

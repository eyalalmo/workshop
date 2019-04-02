using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.Domain
{
    class DBSubscribedUser
    {
        //-idחנויות. מוצרים-id. -nameמשתמשים

        Dictionary<string, SubscribedUser> users;
        Dictionary<string, SubscribedUser> loggedInUser;
        private static DBSubscribedUser instance = null;

        public DBSubscribedUser()
        {
            users = new Dictionary<string, SubscribedUser>();
            loggedInUser = new Dictionary<string, SubscribedUser>();
        
        }
        public static DBSubscribedUser getInstance()
        {
            if (instance == null)
            {
                instance = new DBSubscribedUser();
            }
            return instance;
        }


        public string register(SubscribedUser user)
        {
            if (users.ContainsKey(user.getId()))
            return "id already exist";

            else{
                users.Add(user.getId(), user);
            }
            return "";
        }

        public SubscribedUser getSubscribedUser(string username)
        {
            SubscribedUser user;
            if (!users.TryGetValue(username,out user))
                return null;
            return user; 
        }

        public string login(SubscribedUser user)
        {
            if (loggedInUser.ContainsKey(user.getId()))
            return "id already loggedIn";

            else
            {
                loggedInUser.Add(user.getId(), user);
            }
            return "";
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class DBSubscribedUser
    {
        //-idחנויות. מוצרים-id. -nameמשתמשים

        Dictionary<string, SubscribedUser> users;
        Dictionary<string, SubscribedUser> loggedInUser;

        public DBSubscribedUser()
        {
            users = new Dictionary<string, SubscribedUser>();
            loggedInUser = new Dictionary<string, SubscribedUser>();
        
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class DB
    {
        //-idחנויות. מוצרים-id. -nameמשתמשים

        Dictionary<string, User> users;
        Dictionary<int, Store> stores;
        Dictionary<int, Product> products;
        Dictionary<string, User> loggedInUser;

        public DB()
        {
            users = new Dictionary<string, User>();
            stores = new Dictionary<int, Store>();
            products = new Dictionary<int, Product>();

        }

        public string register(User user)
        {
            if (users.ContainsKey(user.getId()))
            return "id already exist";

            else{
                users.Add(user.getId(), user);
            }
            return "";
        }

        public string login(User user)
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

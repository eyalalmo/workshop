using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.Domain
{
    public class DBSubscribedUser
    {

        Dictionary<string, SubscribedUser> users;
        Dictionary<string, SubscribedUser> loggedInUser;
        private static DBSubscribedUser instance = null;



        private DBSubscribedUser()
        {
            users = new Dictionary<string, SubscribedUser>();
            loggedInUser = new Dictionary<string, SubscribedUser>();
            SubscribedUser admin = new SubscribedUser("admin", "1234", new ShoppingBasket());
            register(admin);
        }

        public void cleanDB()
        {
            users = new Dictionary<string, SubscribedUser>();
            loggedInUser = new Dictionary<string, SubscribedUser>();
            SubscribedUser admin = new SubscribedUser("admin", encryptPassword("1234"), new ShoppingBasket());
            register(admin);
        }

        public static DBSubscribedUser getInstance()
        {
            if (instance == null)
            {
                instance = new DBSubscribedUser();
            }
            return instance;
        }

        public string logout(SubscribedUser sub)
        {
            SubscribedUser user;
            if (!loggedInUser.Remove(sub.getUsername()))
                return "user isnt loggedin";
            return "";
        }

        public string register(SubscribedUser user)
        {
            if (users.ContainsKey(user.getUsername()))
                return "id already exist";

            else
            {
                users.Add(user.getUsername(), user);
            }
            return "";
        }

        public SubscribedUser getSubscribedUser(string username)
        {
            SubscribedUser user;
            if (!users.TryGetValue(username, out user))
                return null;
            return user;
        }

        public string login(SubscribedUser user)
        {
            if (loggedInUser.ContainsKey(user.getUsername()))
                return "id already loggedIn";

            else
            {
                loggedInUser.Add(user.getUsername(), user);
            }
            return "";
        }

        public string remove(SubscribedUser user)
        {
            if (users.Remove(user.getUsername()) == false)
            {
                return " user isnt subscribe";
            }
            return "";
        }
        public SubscribedUser getloggedInUser(string name)
        {
            SubscribedUser user;
            if (!loggedInUser.TryGetValue(name, out user))
                return null;
            return user;
        }

        public string encryptPassword(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }
}

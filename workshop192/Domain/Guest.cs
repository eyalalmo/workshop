using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using workshop192.Domain;


namespace workshop192.Domain
{
    class Guest : UserState
    {
        private DBSubscribedUser dbSubscribedUser;

        public Guest()
        {
            dbSubscribedUser = DBSubscribedUser.getInstance();
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
            return "ERROR: not an admin";
        }

        public String login(String username, String password, Session session)
        {
<<<<<<< HEAD
            SubscribedUser sub = DBSubscribedUser.getInstance().getSubscribedUser(username);

=======
            SubscribedUser sub = dbSubscribedUser.getSubscribedUser(username);
>>>>>>> origin/Yael'sBranch
            if (sub != null)
            {
                if (Equals(sub.getPassword(), password))
                {

                    session.setSubscribedUser(sub);
                    if(Equals(username, "admin"))
                    {
                        session.setState(new Admin());
                    }
                    else
                    {
                        session.setState(new LoggedIn());
                    }
                    
<<<<<<< HEAD
                    return DBSubscribedUser.getInstance().login(sub);

=======
                    return dbSubscribedUser.login(sub);
>>>>>>> origin/Yael'sBranch
                }
                else
                {
                    return "ERROR: password incorrect";
                }
            }
            else
            {
                return "ERROR: username does not exist";
            }
        }

        public string logout(SubscribedUser sub)
        {
            return "ERROR: not logged in";
        }

        public string register(string username, string password, Session session)
        {
            SubscribedUser s = dbSubscribedUser.getSubscribedUser(username);
            if (s != null)
            {
                return "ERROR: username already exists";
            }
            else
            {
<<<<<<< HEAD
                SubscribedUser sub = new SubscribedUser(username, password);
                return DBSubscribedUser.getInstance().register(sub);
=======
                SubscribedUser sub = new SubscribedUser(username, password, session.getShoppingBasket());
                return dbSubscribedUser.register(sub);
>>>>>>> origin/Yael'sBranch
            }
        }

        public string removeUser(string username)
        {
            return "ERROR: not an admin";
        }
=======

namespace workshop192.Domain
{
    class Guest
    {
>>>>>>> origin/bar's_branch
    }
}

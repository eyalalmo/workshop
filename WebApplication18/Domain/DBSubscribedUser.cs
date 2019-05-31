using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using WebApplication18.DAL;
using WebApplication18.Domain;

namespace workshop192.Domain
{
    public class DBSubscribedUser :Connector
    {


        Dictionary<string, SubscribedUser> users;
        Dictionary<string, SubscribedUser> loggedInUser;
        private static DBSubscribedUser instance = null;
        private int id;
        
        private DBSubscribedUser()
        {
            users = new Dictionary<string, SubscribedUser>();
            loggedInUser = new Dictionary<string, SubscribedUser>();
          
        }

        public void init()
        {
            users = new Dictionary<string, SubscribedUser>();
            loggedInUser = new Dictionary<string, SubscribedUser>();
           
        }
        public void addAdmin(string name, string pass)
        {
            SubscribedUser admin = new SubscribedUser(name, encryptPassword(pass), new ShoppingBasket());
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

        public void logout(SubscribedUser sub)
        {
            loggedInUser.Remove(sub.getUsername());
        }

        public void register(SubscribedUser user)
        {
            //prev
            users.Add(user.getUsername(), user);
            // new
            string username = user.getUsername();
            string password = user.getPassword();
            try
            {
                connection.Open();
                var c = connection.Query("SELECT username, password FROM [dbo].[Register] WHERE username=@username ", new { username=username });
                
                if (Enumerable.Count(c) == 0)
               {

                    string sql = "INSERT INTO [dbo].[Register] (username, password)" +
                                                     " VALUES (@username, @password)";
                    connection.Execute(sql, new { username, password });
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connection.Close();
            }
        }

        public SubscribedUser getSubscribedUser(string username)
        {
            if (users.ContainsKey(username))
            {
                return users[username];
            }   
            try
            {
                connection.Open();
                var c1 = connection.Query<RegisterEntry>("SELECT username, password FROM [dbo].[Register] WHERE username=@username ", new { username = username });
                if (Enumerable.Count(c1) == 1)
                {
                    RegisterEntry re = c1.ElementAt(0);
                    string password = re.getPassword();

                    string sql = "SELECT * FROM BasketCart WHERE username=@username;";
                    var c2 = connection.Query<BasketCartEntry>(sql, new { username= username });
                    ShoppingBasket sb = new ShoppingBasket(username);

                    if (Enumerable.Count(c2) > 0)
                    {
                        for (int i = 0; i < Enumerable.Count(c2); i++)
                        {
                            BasketCartEntry bc = c2.ElementAt(i);
                            int storeID = bc.getStoreID();
                            sql = "SELECT * FROM CartProduct WHERE storeID=@storeID;";
                            var c3 = connection.Query<CartProductEntry>(sql, new { storeID = storeID });
                            for (int j=0; j<Enumerable.Count(c3); j++)
                            {
                                CartProductEntry cp = c3.ElementAt(j);
                                int productID = cp.getProductID();
                                int amount = cp.getAmount();
                                Product p = DBProduct.getInstance().getProductByID(productID);
                                sb.addToCart(p, amount);
                            }
                        }
                    }

                    SubscribedUser su = new SubscribedUser(username, password, sb);
                    users.Add(username, su);
                    connection.Close();
                    return su;
                }

                else
                {
                    connection.Close();
                    return null;
                }
            }
            catch (Exception)
            {
                connection.Close();
                return null;
            }
        }

        public void login(SubscribedUser user)
        {
             loggedInUser.Add(user.getUsername(), user);
            string username = user.getUsername();
            string password = user.getPassword();
            try
            {
                connection.Open();
                var c = connection.Query("SELECT username, password FROM [dbo].[Register] WHERE username=@username ", new { username = username });

                if (Enumerable.Count(c) == 0)
                {

                    string sql = "INSERT INTO [dbo].[Register] (username, password)" +
                                                     " VALUES (@username, @password)";
                    connection.Execute(sql, new { username, password });
                    connection.Close();
                }
            }
            catch (Exception)
            {
                connection.Close();
            }

        }

        public void remove(SubscribedUser user)
        {
            users.Remove(user.getUsername());
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

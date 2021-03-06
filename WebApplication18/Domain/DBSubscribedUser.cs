﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using WebApplication18.DAL;
using WebApplication18.Domain;
using WebApplication18.Logs;

namespace workshop192.Domain
{
    public class DBSubscribedUser : Connector
    {
        Dictionary<string, SubscribedUser> users;
        Dictionary<string, SubscribedUser> loggedInUser;
        private static DBSubscribedUser instance = null;
        private int id;

        private DBSubscribedUser()
        {
            users = new Dictionary<string, SubscribedUser>();
            loggedInUser = new Dictionary<string, SubscribedUser>();
            //SubscribedUser admin = new SubscribedUser("admin", encryptPassword("1234"), new ShoppingBasket("admin"));
            //register(admin);
        }

        public void init()
        {
            if (instance == null)
            {
                instance = new DBSubscribedUser();
            }

        }

        public void updateShoppingBasket()
        {
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    foreach (KeyValuePair<string, SubscribedUser> pair in users)
                    {
                        string username = pair.Key;
                        SubscribedUser su = pair.Value;

                        string sql = "SELECT * FROM BasketCart WHERE username=@username;";
                        var c2 = connection.Query<BasketCartEntry>(sql, new { username = username });
                        ShoppingBasket sb = su.getShoppingBasket();
                        if (Enumerable.Count(c2) > 0)
                        {
                            for (int i = 0; i < Enumerable.Count(c2); i++)
                            {
                                BasketCartEntry bc = c2.ElementAt(i);
                                int storeID = bc.getStoreID();
                                sql = "SELECT * FROM CartProduct WHERE storeID=@storeID AND username=@username;";
                                var c3 = connection.Query<CartProductEntry>(sql, new { storeID, username });

                                for (int j = 0; j < Enumerable.Count(c3); j++)
                                {
                                    CartProductEntry cp = c3.ElementAt(j);
                                    int productID = cp.getProductID();
                                    int amount = cp.getAmount();
                                    Product p = DBProduct.getInstance().getProductByID(productID);
                                    sb.addToCartNoDBUpdate(p, amount, storeID);
                                }
                            }
                        }
                        List<StoreRole> storeRoles = su.getStoreRoles();

                        foreach (StoreRole sr in DBStore.getInstance().getAllStoreRoles(username))
                        {
                            if (sr.getUser().getUsername() == username)
                            {
                                storeRoles.Add(sr);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                connection.Close();
            }
        }

        public void addAdmin(string name, string pass)
        {
            SubscribedUser admin = new SubscribedUser(name, pass, new ShoppingBasket());
            register(admin);
        }

        public void initTests()
        {
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    connection.Execute("DELETE FROM Register");
                    connection.Execute("DELETE FROM BasketCart");
                    connection.Execute("DELETE FROM CartProduct");
                    instance = new DBSubscribedUser();
                    connection.Close();
                }

            }
            catch (Exception e)
            {
                connection.Close();
            }
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
            users.Add(user.getUsername(), user);
            string username = user.getUsername();
            string password = user.getPassword();
            try
            {
                lock (connection)
                {
                    connection.Open();
                    //SqlConnection connection = Connector.getInstance().getSQLConnection();
                    var c = connection.Query("SELECT username, password FROM [dbo].[Register] WHERE username=@username ", new { username = username });
                    //connection.Close();
                    if (Enumerable.Count(c) == 0)
                    {
                        string sql = "INSERT INTO [dbo].[Register] (username, password)" +
                                                        " VALUES (@username, @password)";
                        connection.Execute(sql, new { username, password });
                    }
                    connection.Close();
                }

            }
            catch (Exception e)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function register in db subscribed user, user name: " + user.getUsername());
                throw new ConnectionException();
            }
        }

        public SubscribedUser getSubscribedUserForInitStore(string username)
        {
            if (users.ContainsKey(username))
            {
                return users[username];
            }
            try
            {
                lock (connection)
                {
                    connection.Open();
                    //SqlConnection connection = Connector.getInstance().getSQLConnection();
                    var c1 = connection.Query<RegisterEntry>("SELECT username, password FROM [dbo].[Register] WHERE username=@username ", new { username = username });
                    connection.Close();
                    if (Enumerable.Count(c1) == 1)
                    {
                        RegisterEntry re = c1.ElementAt(0);
                        string password = re.getPassword();

                        ShoppingBasket sb = new ShoppingBasket(username);
                        SubscribedUser su = new SubscribedUser(username, password, sb);

                        users.Add(username, su);
                        return su;
                    }

                    else
                    {
                        return null;
                    }
                }

            }
            catch (Exception e)
            {
                connection.Close();
                return null;
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
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    var c1 = connection.Query<RegisterEntry>("SELECT username, password FROM [dbo].[Register] WHERE username=@username ", new { username = username });

                    if (Enumerable.Count(c1) == 1)
                    {
                        RegisterEntry re = c1.ElementAt(0);
                        string password = re.getPassword();
                        string sql = "SELECT * FROM BasketCart WHERE username=@username;";
                        var c2 = connection.Query<BasketCartEntry>(sql, new { username = username });
                        ShoppingBasket sb = new ShoppingBasket(username);

                        SubscribedUser su = new SubscribedUser(username, password, sb);
                        List<StoreRole> storeRoles = su.getStoreRoles();
                        users.Add(username, su);
                        DBStore.getInstance().getAllStoreRoles(username);

                        if (Enumerable.Count(c2) > 0)
                        {
                            for (int i = 0; i < Enumerable.Count(c2); i++)
                            {
                                BasketCartEntry bc = c2.ElementAt(i);
                                int storeID = bc.getStoreID();
                                sql = "SELECT * FROM CartProduct WHERE storeID=@storeID AND username=@username;";

                                var c3 = connection.Query<CartProductEntry>(sql, new { storeID, username });
                                for (int j = 0; j < Enumerable.Count(c3); j++)
                                {
                                    CartProductEntry cp = c3.ElementAt(j);
                                    int productID = cp.getProductID();
                                    int amount = cp.getAmount();
                                    Product p = DBProduct.getInstance().getProductByID(productID);
                                    sb.addToCartNoDBUpdate(p, amount, storeID);
                                }
                            }
                        }

                        //foreach (StoreRole sr in DBStore.getInstance().getAllStoreRoles(username))
                        //{
                        //    if(sr.getUser().getUsername()==username)
                        //    {
                        //        storeRoles.Add(sr);
                        //    }
                        //}
                        //users.Add(username, su);
                        connection.Close();
                        return su;
                    }

                    else
                    {
                        connection.Close();
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                connection.Close();
                return null;
            }
        }

        public void updateStoreRole(SubscribedUser user)
        {
            string username = user.getUsername();
            foreach (StoreRole sr in DBStore.getInstance().getAllStoreRoles(username))
            {
                user.addStoreRole(sr);
            }
        }

        public void login(SubscribedUser user)
        {
            loggedInUser[user.getUsername()] = user;
            string username = user.getUsername();
            string password = user.getPassword();
            try
            {
                lock (connection)
                {
                    connection.Open();
                    //SqlConnection connection = Connector.getInstance().getSQLConnection();
                    var c = connection.Query("SELECT username, password FROM [dbo].[Register] WHERE username=@username ", new { username = username });
                    //connection.Close();
                    if (Enumerable.Count(c) == 0)
                    {
                        throw new LoginException("Username " + user.getUsername() + "does not exist");
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                if (e is ClientException)
                    throw e;
                else
                {
                    connection.Close();
                    SystemLogger.getErrorLog().Error("Connection error in function login in db subscribed user, user name: " + user.getUsername());
                    throw new ConnectionException();
                }
            }

        }

        public void addCartToBasketCartTable(string username, int storeID)
        {
            string sql = "INSERT INTO [dbo].[BasketCart] (username, storeID)" +
                                                   " VALUES (@username, @storeID)";
            try
            {
                lock (connection)
                {
                    connection.Open();
                    //SqlConnection connection = Connector.getInstance().getSQLConnection();
                    connection.Execute(sql, new { username, storeID });
                    connection.Close();
                }
            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function add cart to basket cart table in db subscribed user");
                throw new ConnectionException();
            }
        }

        public void addProductToCartProductTable(string username, int storeID, int productID, int amount)
        {
            string sql = "INSERT INTO [dbo].[CartProduct] (username, productID, storeID, amount)" +
                                                               " VALUES (@username, @productID, @storeID, @amount)";
            try
            {
                // SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    connection.Execute(sql, new { username, productID, storeID, amount });
                    connection.Close();
                }
                //connection.Close();

            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function add product to cart product table in db subscribed user");
                throw new ConnectionException();
            }
        }

        public void remove(SubscribedUser user)
        {
            string username = user.getUsername();
            if (loggedInUser.ContainsKey(username))
                loggedInUser.Remove(username);
            users.Remove(username);
            string sql1 = "DELETE FROM Register WHERE username=@username";
            string sql2 = "DELETE FROM BasketCart WHERE username=@username";
            string sql3 = "DELETE FROM CartProduct WHERE username=@username";
            string sql4 = "DELETE FROM PendingOwners WHERE username=@username";
            string sql5 = "DELETE FROM Contracts WHERE username=@username";

            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        connection.Execute(sql1, new { username }, transaction);
                        connection.Execute(sql2, new { username }, transaction);
                        connection.Execute(sql3, new { username }, transaction);
                        connection.Execute(sql4, new { username }, transaction);
                        connection.Execute(sql5, new { username }, transaction);
                        transaction.Commit();
                    }
                    connection.Close();
                }
                //connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function remove in db subscribed user while removing " + user.getUsername());
                throw new ConnectionException();
            }
        }
        public SubscribedUser getloggedInUser(string name)
        {
            SubscribedUser user;
            if (!loggedInUser.TryGetValue(name, out user))
                return null;
            return user;
        }

        public void removeProductFromCartProductTable(string username, int storeID, int productId)
        {
            string sql = "DELETE FROM [dbo].[CartProduct] WHERE username=@username AND productID=@productId AND storeID=@storeID";
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    connection.Execute(sql, new { username, productId, storeID });
                    connection.Close();
                }
                //connection.Close();

            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function remove product from cart product table in dbsubscribed user ");
                throw new ConnectionException();
            }
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

        public void deleteCartFromBasketCartTable(string username, int storeID)
        {
            string sql = "DELETE FROM [dbo].[BasketCart]  WHERE username =@username AND storeID =@storeID";
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    connection.Execute(sql, new { username, storeID });
                    connection.Close();
                }
                //connection.Close();

            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function deleteCartFromBasket in dbsubscribed user ");
                throw new ConnectionException();
            }
        }

        public void updateAmountOnCartProductTable(string username, int storeID, int productID, int newAmount)
        {
            string sql = "UPDATE CartProduct SET amount =@newAmount WHERE username = @username AND productID =@productID AND storeID =@storeID;";
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    connection.Execute(sql, new { newAmount, username, productID, storeID });
                    connection.Close();
                }
                //connection.Close();
            }

            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function updateAmountOnCartProductTable in dbsubscribed user ");
                throw new ConnectionException();
            }

        }

        public void updateTablesAfterPurchase(string username, Dictionary<int, ShoppingCart> shoppingCarts)
        {
            string sql1 = "DELETE FROM BasketCart WHERE username=@username";
            string sql2 = "DELETE FROM CartProduct WHERE username=@username AND storeID=@storeID";
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                       
                        connection.Execute(sql1, new { username }, transaction);

                        //connection.Close();
                        foreach (KeyValuePair<int, ShoppingCart> pair in shoppingCarts)
                        {
                            int storeID = pair.Key;
                            connection.Execute(sql2, new { username, storeID }, transaction);
                        }
                        transaction.Commit();
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function updateTablesAfterPurchase in DBSubscribed user ");
                throw new ConnectionException();

            }

        }
    }
}

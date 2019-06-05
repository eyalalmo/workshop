using System;
using System.Collections.Generic;
using System.Linq;
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
                           connection.Close();
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

                    foreach (StoreRole sr in DBStore.getInstance().getAllStoreRoles())
                    {
                        if (sr.getUser().getUsername() == username)
                        {
                            storeRoles.Add(sr);
                        }
                    }
                }
           }
           catch (Exception)
           {
               connection.Close();
           }

       }

       public void addAdmin(string name, string pass)
       {
           SubscribedUser admin = new SubscribedUser(name, encryptPassword(pass), new ShoppingBasket());
           register(admin);
       }

        public void initTests()
        {
            
                try
                {
                connection.Open();
                connection.Execute("DELETE FROM Register");
                connection.Execute("DELETE FROM BasketCart");
                connection.Execute("DELETE FROM CartProduct");
                connection.Close();
                instance = new DBSubscribedUser();

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
               connection.Open();
               var c = connection.Query("SELECT username, password FROM [dbo].[Register] WHERE username=@username ", new { username = username });

               if (Enumerable.Count(c) == 0)
               {

                   string sql = "INSERT INTO [dbo].[Register] (username, password)" +
                                                    " VALUES (@username, @password)";
                   connection.Execute(sql, new { username, password });
               }

               connection.Close();
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               connection.Close();
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
               connection.Open();
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
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                connection.Open();
                var c1 = connection.Query<RegisterEntry>("SELECT username, password FROM [dbo].[Register] WHERE username=@username ", new { username = username });
                connection.Close();
                if (Enumerable.Count(c1) == 1)
                {
                    RegisterEntry re = c1.ElementAt(0);
                    string password = re.getPassword();
                    connection.Open();
                    string sql = "SELECT * FROM BasketCart WHERE username=@username;";
                    var c2 = connection.Query<BasketCartEntry>(sql, new { username= username });
                    connection.Close();
                    ShoppingBasket sb = new ShoppingBasket(username);

                    if (Enumerable.Count(c2) > 0)
                    {
                        for (int i = 0; i < Enumerable.Count(c2); i++)
                        {
                            BasketCartEntry bc = c2.ElementAt(i);
                            int storeID = bc.getStoreID();
                            connection.Open();
                            sql = "SELECT * FROM CartProduct WHERE storeID=@storeID AND username=@username;";
                           
                            var c3 = connection.Query<CartProductEntry>(sql, new { storeID, username });
                            connection.Close();
                            for (int j=0; j<Enumerable.Count(c3); j++)
                            {
                                CartProductEntry cp = c3.ElementAt(j);
                                int productID = cp.getProductID();
                                int amount = cp.getAmount();
                                Product p = DBProduct.getInstance().getProductByID(productID);
                                sb.addToCartNoDBUpdate(p, amount, storeID);
                            }
                        }
                    }
                    SubscribedUser su = new SubscribedUser(username, password, sb);
                    List<StoreRole> storeRoles = su.getStoreRoles();

                    foreach (StoreRole sr in DBStore.getInstance().getAllStoreRoles())
                    {
                        if(sr.getUser().getUsername()==username)
                        {
                            storeRoles.Add(sr);
                        }
                    }
                    users.Add(username, su);
                    return su;
                }

                else
                {
                    connection.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connection.Close();
                return null;
            }
        }//
        public void updateStoreRole(SubscribedUser user)
        {
            string username = user.getUsername();
            foreach (StoreRole sr in DBStore.getInstance().getRolesByUserName(username))
            {
                user.addStoreRole(sr);
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
                    
                }
                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
            }

        }

        public void addCartToBasketCartTable(string username, int storeID)
        {
            string sql = "INSERT INTO [dbo].[BasketCart] (username, storeID)" +
                                                   " VALUES (@username, @storeID)";
            try
            {
                connection.Open();
                connection.Execute(sql, new { username, storeID });
                connection.Close();

            }
            catch (Exception e)
            {
                connection.Close();
            }
        }

        public void addProductToCartProductTable(string username, int storeID, int productID, int amount)
        {
            string sql = "INSERT INTO [dbo].[CartProduct] (username, productID, storeID, amount)" +
                                                               " VALUES (@username, @productID, @storeID, @amount)";
            try
            {
                connection.Open();
                connection.Execute(sql, new { username, productID, storeID, amount });
                connection.Close();

            }
            catch (Exception)
            {
                connection.Close();
            }
        }

        public void remove(SubscribedUser user)
        {
            string username = user.getUsername();
            if (loggedInUser.ContainsKey(username))
                loggedInUser.Remove(username);
            users.Remove(username);
            string sql1 = "DELETE * FROM Register WHERE username =@username";
            string sql2 = "DELETE * FROM BasketCart WHERE username=@username";
            string sql3 = "DELETE * FROM CartProduct WHERE username=@username";
            
            try
            {
                connection.Open();
                connection.Execute(sql1, new { username });
                connection.Execute(sql2, new { username });
                connection.Execute(sql3, new { username });
                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
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
            string sql = "DELETE FROM [dbo].[CartProduct] WHERE username =@username producdID =@productID AND storeID =@storeID";
            try
            {
                connection.Open();
                connection.Execute(sql, new { username, productId, storeID });
                connection.Close();

            }
            catch (Exception)
            {
                connection.Close();
            }
        }

       
        public void deleteCartFromBasketCartTable(string username, int storeID)
        {
            string sql = "DELETE FROM [dbo].[BasketCart]  WHERE username =@username AND storeID =@storeID";
            try
            {
                connection.Open();
                connection.Execute(sql, new { username, storeID });
                connection.Close();

            }
            catch (Exception)
            {
                connection.Close();
            }
        }

        public void updateAmountOnCartProductTable(string username,  int storeID, int productID, int newAmount)
        {
            string sql = "UPDATE CartProduct SET amount =@newAmount WHERE username = @username AND productID =@productID AND storeID =@storeID;";
            try
            {
                connection.Open();
                connection.Execute(sql, new {newAmount,username, productID, storeID });
                connection.Close();

            }
            catch (Exception)
            {
                connection.Close();
            }

        }

        public void updateTablesAfterPurchase(string username, Dictionary<int, ShoppingCart> shoppingCarts)
        {
            string sql1 = "DELETE FROM BasketCart WHERE username=@username";
            string sql2 = "DELETE FROM CartProduct WHERE username=@username AND storeID=@storeID";
            try
            {
                connection.Open();
                connection.Execute(sql1, new { username });
                foreach(KeyValuePair<int, ShoppingCart> pair in shoppingCarts)
                {
                    int storeID = pair.Key;
                    connection.Execute(sql2, new { username, storeID });
                }

                connection.Close();

            }
            catch (Exception)
            {
                connection.Close();
            }
            
        }
    }
}

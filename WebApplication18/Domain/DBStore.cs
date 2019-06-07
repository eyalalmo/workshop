﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication18.DAL;
using Dapper;
using WebApplication18.Domain;

namespace workshop192.Domain
{
    public class DBStore : Connector
    {
        public static DBStore instance;
        public LinkedList<Store> stores;
        public LinkedList<StoreRole> storeRole;
        public static int nextStoreID = 0;


        private DBStore()
        {
            storeRole = new LinkedList<StoreRole>();
            // stores = initStores();
            stores = new LinkedList<Store>();
            nextStoreID = getUpdatedId();
        }
            if (IsTestsMode.isTest == false)
            {
                storeRole = new LinkedList<StoreRole>();
                stores = initStores();
                nextStoreID = getUpdatedId();
            }
            else
            {
                storeRole = new LinkedList<StoreRole>();
                stores = new LinkedList<Store>();

            }
        }
        public static DBStore getInstance()
        {
            if (instance == null)
            {
                instance = new DBStore();

            }

            return instance;
        }

        public void init()
        {
            //init both
            if (instance == null)
                instance = new DBStore();

        }

        public void initTests()
        {
            try
            {
                connection.Open();
                connection.Execute("DELETE FROM Stores");
                connection.Execute("DELETE FROM StoreRoles");
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
            }
        }


        //private LinkedList<Store> initStores()
        //{
        //try
        //{
        //    connection.Open();
        //    LinkedList<Store> newStores = new LinkedList<Store>();
        //    var StoreResult = connection.Query<StoreEntry>("SELECT * FROM [dbo].[Stores] ");
        //    var StoreRoleResult = connection.Query<StoreRoleEntry>("SELECT * FROM [dbo].[StoreRoles] ");
        //    connection.Close();
        //    for (int i = 0; i < StoreResult.Count(); i++)
        //    {
        //        StoreEntry se = StoreResult.ElementAt(i);
        //        Store s = new Store(se.getStoreId(), se.getName(), se.getDescription());
        //        LinkedList<Product> lst= DBProduct.getInstance().getAllProducts();
        //        foreach(Product p in lst)
        //        {
        //            if (p.getStoreID() == s.getStoreID())
        //                s.addProduct(p);
        //        }
        //        if(se.getMaxPurchasePolicy()!=-1)
        //            s.setMaxPurchasePolicy(se.getMaxPurchasePolicy());
        //        if (se.getMinPurchasePolicy() != -1)
        //            s.setMinPurchasePolicy(se.getMinPurchasePolicy());
        //        foreach (StoreRoleEntry element in StoreRoleResult)
        //        {
        //            if (element.getStoreId() == s.getStoreID() && element.getIsOwner() == 1)
        //            {
        //                SubscribedUser appointedBy = null;
        //                try
        //                {
        //                    appointedBy = DBSubscribedUser.getInstance().getSubscribedUserForInitStore(element.getAppointedBy());
        //                }
        //                catch (Exception) { }
        //                    SubscribedUser user = DBSubscribedUser.getInstance().getSubscribedUserForInitStore(element.getUserName());
        //                StoreOwner so = new StoreOwner(appointedBy, user, s);
        //                s.addStoreRoleFromInitOwner(so);
        //                storeRole.AddLast(so);
        //                newStores.AddLast(s);

        //            }
        //            else if (element.getStoreId() == s.getStoreID() && element.getIsOwner() == 0)
        //            {
        //                SubscribedUser appointedBy = DBSubscribedUser.getInstance().getSubscribedUser(element.getAppointedBy());
        //                SubscribedUser user = DBSubscribedUser.getInstance().getSubscribedUser(element.getUserName());
        //                Permissions p = new Permissions(false, false, false);
        //                if (element.getEditDiscount() == 1)
        //                    p.setEditDiscount(true);
        //                if (element.getEditPolicy() == 1)
        //                    p.setEditPolicy(true);
        //                if (element.getEditProduct() == 1)
        //                    p.setEditProduct(true);
        //                StoreManager sm = new StoreManager(appointedBy, s, user, p);
        //                s.addStoreRoleFromInitManager(sm);
        //                storeRole.AddLast(sm);
        //                newStores.AddLast(s);
        //            }
        //        }

        //    }

        //    return newStores;
        //}
        //catch(Exception)
        //{
        //   // StackTrace = "   ב-  System.Collections.Generic.Dictionary`2.FindEntry(TKey key)\r\n   ב-  System.Collections.Generic.Dictionary`2.ContainsKey(TKey key)\r\n   ב-  workshop192.Domain.DBSubscribedUser.getSubscribedUser(String username) ב- C:\\Users\\etay2\\Desktop\\C#Work...
        //    connection.Close();
        //    throw new StoreException("cant init");
        //}

        //}


        public void cleanDB()
        {
            stores = new LinkedList<Store>();
            storeRole = new LinkedList<StoreRole>();
            nextStoreID = 0;
        }

        public LinkedList<StoreRole> getRolesByUserName(string username)
        {
            initStoresAndRolesForUserName(username);
            LinkedList<StoreRole> lst = new LinkedList<StoreRole>();

            foreach (StoreRole st in storeRole)
            {
                /////////////////////check
                if (st.getUser() != null && st.getUser().getUsername() == username)
                    lst.AddLast(st);
            }
            return lst;
        }

        public void removeStoreRole(StoreRole sr)
        {
            try
            {
                connection.Open();
                int storeId = sr.getStore().getStoreID();
                string userName = sr.getUser().getUsername();
                connection.Execute("DELETE FROM StoreRoles WHERE  storeId=@storeId AND userName=@userName", new { storeId, userName });
                storeRole.Remove(sr);
                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("cant remove role");
            }

        }

        //public void removeStoreRole(Store store, SubscribedUser user)
        //{
        //    StoreRole sr = getStoreRole(store, user);
        //    if (sr != null)
        //        storeRole.Remove(sr);

        //}

        public StoreRole getStoreRole(Store store, SubscribedUser user)
        {
            initStoresAndRolesForUserName(user.getUsername());
            foreach (StoreRole st in storeRole)
            {
                Store s = st.getStore();
                SubscribedUser u = st.getUser();
                if (store.Equals(s) && user.Equals(u))
                    return st;
            }

            return null;
        }


        public void addStoreRole(StoreRole sr)
        {
            try
            {
                //         public SubscribedUser appointedBy;
                //private Store store;
                //public SubscribedUser user;
                //public Permissions permissions;
                //public bool isOwner = false;

                ///
                //per.Add("editProduct", editProduct);
                //per.Add("editDiscount", editDiscount);
                //per.Add("editPolicy", editPolicy);

                ////
                connection.Open();

                string sql = "INSERT INTO [dbo].[StoreRoles] (storeId, appointedBy,userName,isOwner,editProduct,editDiscount,editPolicy)" +
                                 " VALUES (@storeId, @appointedBy, @userName,@isOwner,@editProduct,@editDiscount,@editPolicy)";
                int storeId = sr.getStore().getStoreID();
                string appointedBy = null;
                if (sr.getAppointedBy() != null)
                    appointedBy = sr.getAppointedBy().getUsername();
                string userName = sr.getUser().getUsername();
                int isOwner = sr.getIsOwner();
                int editProduct = 1;
                int editDiscount = 1;
                int editPolicy = 1;
                if (isOwner == 0)
                {
                    editProduct = sr.GetPermissions().getPermission("editProduct");
                    editDiscount = sr.GetPermissions().getPermission("editDiscount");
                    editPolicy = sr.GetPermissions().getPermission("editPolicy");
                }
                connection.Execute(sql, new { storeId, appointedBy, userName, isOwner, editProduct, editDiscount, editPolicy });
                storeRole.AddFirst(sr);
                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("cant add store roll");
            }
        }

        public void removeOwnerNumerByOne(int storeId, int numOfOwners)
        {
            connection.Open();
            connection.Execute("UPDATE [dbo].[Stores] SET numOfOwners = @newNumber WHERE storeId = @storeId", new { storeId = storeId, newNumber = numOfOwners });
            connection.Close();
        }

        public void addownerNumerByOne(int storeId, int newNumber)
        {
            try
            {
                connection.Open();
                connection.Execute("UPDATE [dbo].[Stores] SET numOfOwners = @newNumber WHERE storeId = @storeId", new { storeId = storeId, newNumber = newNumber });
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
            }
        }

        public int addStore(Store store)
        {
            try
            {
                //////////////////////////////////
                //        public int storeId;
                // string name;
                // string description;
                //public LinkedList<Product> productList;
                //public List<StoreRole> roles;
                //public int numOfOwners;
                //public bool active;
                //public LinkedList<DiscountComponent> discountList;
                //public MinAmountPurchase minPurchasePolicy;
                //public MaxAmountPurchase maxPurchasePolicy;
                //public LinkedList<InvisibleDiscount> invisibleDiscountList;
                //public Dictionary<string, HashSet<string>> pendingOwners;


                ////////////////////////////////////////////////
                connection.Open();

                string sql = "INSERT INTO [dbo].[Stores] (storeId, name,description,numOfOwners,active,minPurchasePolicy,maxPurchasePolicy)" +
                                 " VALUES (@storeId, @name, @description,@numOfOwners,@active,@minPurchasePolicy,@maxPurchasePolicy)";
                int storeId = store.getStoreID();
                string name = store.getStoreName();
                string description = store.getDescription();
                int numOfOwners = store.getNumberOfOwners();
                int active = 0;
                if (store.isActive() == true)
                    active = 1;
                int minPurchasePolicy = -1;
                try
                {
                    minPurchasePolicy = store.getMinAmountPolicy().getAmount();
                }
                catch (Exception) { }
                int maxPurchasePolicy = -1;
                try
                {
                    maxPurchasePolicy = store.getMaxAmountPolicy().getAmount();
                }
                catch (Exception) { }

                connection.Execute(sql, new { storeId, name, description, numOfOwners, active, minPurchasePolicy, maxPurchasePolicy });

                connection.Close();
                stores.AddFirst(store);


                /////////////////////////
                return store.getStoreID();
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("faild to add store");
            }
        }

        public Store getStore(int storeId)
        {

            foreach (Store s in stores)
            {
                if (s.getStoreID() == storeId)
                    return s;
            }
            /////////////////////////////////////////////////////
            try
            {
                connection.Open();
                LinkedList<Store> newStores = new LinkedList<Store>();
                var StoreResult = connection.Query<StoreEntry>("SELECT * FROM [dbo].[Stores] WHERE storeId=@storeId ", new { storeId = storeId });
                var StoreRoleResult = connection.Query<StoreRoleEntry>("SELECT * FROM [dbo].[StoreRoles] WHERE storeId=@storeId ", new { storeId = storeId });
                connection.Close();

                StoreEntry se = StoreResult.ElementAt(0);
                Store s = new Store(se.getStoreId(), se.getName(), se.getDescription());
                LinkedList<Product> lst = DBProduct.getInstance().getAllProducts();
                foreach (Product p in lst)
                {
                    if (p.getStoreID() == s.getStoreID())
                        s.addProduct(p);
                }
                if (se.getMaxPurchasePolicy() != -1)
                    s.setMaxPurchasePolicy(se.getMaxPurchasePolicy());
                if (se.getMinPurchasePolicy() != -1)
                    s.setMinPurchasePolicy(se.getMinPurchasePolicy());
                foreach (StoreRoleEntry element in StoreRoleResult)
                {
                    if (element.getStoreId() == s.getStoreID() && element.getIsOwner() == 1)
                    {
                        SubscribedUser appointedBy = null;
                        try
                        {
                            appointedBy = DBSubscribedUser.getInstance().getSubscribedUserForInitStore(element.getAppointedBy());
                        }
                        catch (Exception) { }
                        SubscribedUser user = DBSubscribedUser.getInstance().getSubscribedUserForInitStore(element.getUserName());
                        StoreOwner so = new StoreOwner(appointedBy, user, s);
                        s.addStoreRoleFromInitOwner(so);
                        storeRole.AddLast(so);


                    }
                    else if (element.getStoreId() == s.getStoreID() && element.getIsOwner() == 0)
                    {
                        SubscribedUser appointedBy = DBSubscribedUser.getInstance().getSubscribedUser(element.getAppointedBy());
                        SubscribedUser user = DBSubscribedUser.getInstance().getSubscribedUser(element.getUserName());
                        Permissions p = new Permissions(false, false, false);
                        if (element.getEditDiscount() == 1)
                            p.setEditDiscount(true);
                        if (element.getEditPolicy() == 1)
                            p.setEditPolicy(true);
                        if (element.getEditProduct() == 1)
                            p.setEditProduct(true);
                        StoreManager sm = new StoreManager(appointedBy, s, user, p);
                        s.addStoreRoleFromInitManager(sm);
                        storeRole.AddLast(sm);

                    }
                }
                stores.AddLast(s);
                return s;
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("cant return store");
            }


            ///////////////////////////////////////////////////

        }
        public void removeStore(Store store)
        {
            try
            {
                if (!stores.Contains(store))
                {
                    throw new DoesntExistException("Error: Store " + store.getStoreName() + " doesn't exist");
                }

                ///////
                connection.Open();
                int storeId = store.getStoreID();
                var affectedRows = connection.Execute("DELETE FROM Stores WHERE  storeId=@storeId ", new { hash = storeId });

                connection.Close();


                /////
                stores.Remove(store);
                foreach (StoreRole st in storeRole)
                {
                    if (st.getStore().getStoreID() == store.getStoreID())
                        storeRole.Remove(st);
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("cant delete store");
            }
        }

        public void closeStore(Store s)
        {
            if (stores.Contains(s))
            {

                s.closeStore();
                foreach (StoreRole sr in storeRole)
                {
                    //if (sr.getStore().getStoreID() == s.getStoreID())
                    //    sr.getUser().notify();
                }
            }
        }


        public int getNextStoreID()
        {
            try
            {
                connection.Open();
                int idNum = connection.Query<int>("SELECT id FROM [dbo].[IDS] WHERE type=@type ", new { type = "store" }).First();
                int next = idNum + 1;
                connection.Execute("UPDATE [dbo].[IDS] SET id = @id WHERE type = @type", new { id = next, type = "store" });
                nextStoreID = idNum;
                connection.Close();
                return idNum;
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("connection to db faild");
            }

        }

        //if owner -> close store and remove store role, if manager only removes store role
        public void removeStoreByUser(SubscribedUser user)
        {
            foreach (StoreRole sr in storeRole)
                if ((sr.getUser()).getUsername() == user.getUsername())
                {
                    if (sr is StoreOwner)
                    {
                        closeStore(sr.getStore());
                    }
                    storeRole.Remove(sr);
                }
        }

        public LinkedList<StoreRole> getAppointedByList(StoreRole sRole)
        {
            LinkedList<StoreRole> res = new LinkedList<StoreRole>();

            foreach (StoreRole sr in storeRole)
            {
                if (sr.getAppointedBy().Equals(sRole))
                    res.AddFirst(sr);
            }
            return res;
        }

        public List<StoreRole> getRoles(int id)
        {
            return getStore(id).getRoles();
        }

        public int getUpdatedId()
        {
            try
            {
                connection.Open();
                int idNum = connection.Query<int>("SELECT id FROM [dbo].[IDS] WHERE type=@type ", new { type = "store" }).First();
                connection.Close();
                return idNum;
            }
            catch (Exception e)
            {
                connection.Close();
                throw new StoreException("cant connect");
            }
        }

        public void initStoresAndRolesForUserName(string userName)
        {
            LinkedList<StoreRoleEntry> sr = getRolesEntryByUserName(userName);
             
            foreach (StoreRoleEntry role in sr)
            {
                int id = role.getStoreId();
                getStore(id);
            }

        }
        public LinkedList<StoreRoleEntry> getRolesEntryByUserName(string userName)
        {
            try
            {
                connection.Open();
                LinkedList<StoreRoleEntry> lst = new LinkedList<StoreRoleEntry>();
                var StoreRoleResult = connection.Query<StoreRoleEntry>("SELECT * FROM [dbo].[StoreRoles] WHERE userName=@userName ", new { userName = userName });
                connection.Close();
                foreach (StoreRoleEntry element in StoreRoleResult)
                {
                    lst.AddLast(element);
                }
                return lst;
            }
            catch(Exception e)
            {
                connection.Close();
                throw new StoreException("cant get roles from db");
            }
        }
    }
}
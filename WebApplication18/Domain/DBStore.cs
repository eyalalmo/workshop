﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication18.DAL;
using Dapper;
using WebApplication18.Domain;
using System.Data.SqlClient;
using WebApplication18.Logs;

namespace workshop192.Domain
{
    public class DBStore : Connector
    {
        private SqlTransaction ownerTrans = null;
        public static DBStore instance;
        public LinkedList<Store> stores;
        public LinkedList<StoreRole> storeRole;

        

        public static int nextStoreID = 0;
        public static int nextPolicyID = 0;

        private DBStore()
        {
            if (MarketSystem.testsMode == false)
            {
                storeRole = new LinkedList<StoreRole>();
                stores = new LinkedList<Store>();
                nextStoreID = getUpdatedId();
                nextPolicyID = getUpdatedPolicyID();
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

            stores = new LinkedList<Store>();
            storeRole = new LinkedList<StoreRole>();
            nextStoreID = 0;
            //init both
            if (instance == null)
                instance = new DBStore();

        }

        public void initTests()
        {
            storeRole = new LinkedList<StoreRole>();
            stores = new LinkedList<Store>();
            try
            {
                lock (connection)
                {
                    connection.Open();
                    //SqlConnection connection = Connector.getInstance().getSQLConnection();
                    connection.Execute("DELETE FROM Stores");
                    connection.Execute("DELETE FROM StoreRoles");
                    connection.Execute("DELETE FROM Contracts");
                    connection.Execute("DELETE FROM PendingOwners");
                    connection.Execute("DELETE FROM PurchasePolicy");
                    connection.Execute("UPDATE [dbo].[IDS] SET id = 0 WHERE type = 'store'");
                    connection.Execute("UPDATE [dbo].[IDS] SET id = 0 WHERE type = 'policy'");
                    connection.Close();
                }
                //connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }



        //private LinkedList<Store> initStores()
        //{
        //try
        //{
        //    SqlConnection connection = Connector.getInstance().getSQLConnection();
        //    LinkedList<Store> newStores = new LinkedList<Store>();
        //    var StoreResult = connection.Query<StoreEntry>("SELECT * FROM [dbo].[Stores] ");
        //    var StoreRoleResult = connection.Query<StoreRoleEntry>("SELECT * FROM [dbo].[StoreRoles] ");
        //    //connection.Close();
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
        //    //connection.Close();
        //    throw new StoreException("cant init");
        //}

        //}

        public void cleanDB()
        {
            stores = new LinkedList<Store>();
            storeRole = new LinkedList<StoreRole>();
            nextStoreID = 0;
            nextPolicyID = 0;
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
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                int storeId = sr.getStore().getStoreID();
                string userName = sr.getUser().getUsername();
                lock (connection)
                {
                    connection.Open();
                    connection.Execute("DELETE FROM StoreRoles WHERE  storeId=@storeId AND userName=@userName", new { storeId, userName });
                    storeRole.Remove(sr);
                    connection.Close();
                }
            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function remove role in DB Store while removing " + sr.getUser().getUsername());
                throw new ConnectionException();
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

            //SqlConnection connection = Connector.getInstance().getSQLConnection();
            lock (connection)
            {
                if (ownerTrans == null)
                    connection.Open();

                SqlTransaction transaction;
                if (ownerTrans != null)
                {
                    transaction = ownerTrans;
                }
                else
                    transaction = connection.BeginTransaction();

                try
                {



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
                    connection.Execute(sql, new { storeId, appointedBy, userName, isOwner, editProduct, editDiscount, editPolicy }, transaction);
                    storeRole.AddFirst(sr);
                    if (ownerTrans == null)
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        connection.Close();
                    }
                }
                catch (Exception e)
                { 
                    if (ownerTrans == null)
                    {
                        transaction.Dispose();
                        connection.Close();
                    }
                    SystemLogger.getErrorLog().Error("Connection error in function add role in DB Store while adding " + sr.getUser().getUsername());
                    throw new ConnectionException();
                }
            }
        }
        
        public LinkedList<StoreRole> getAllStoreRoles(string username)
        {
            LinkedList<StoreRole> result = new LinkedList<StoreRole>();
            initStoresAndRolesForUserName(username);
            foreach (StoreRole sr in storeRole)
            {
                if (sr.getUser().getUsername() == username)
                {
                    result.AddLast(sr);
                }

            }
            return result;

        }

        public void removeOwnerNumerByOne(int storeId, int numOfOwners)
        {
            try
            {
                lock (connection)
                {

                    connection.Open();

                    //SqlConnection connection = Connector.getInstance().getSQLConnection();
                    connection.Execute("UPDATE [dbo].[Stores] SET numOfOwners = @newNumber WHERE storeId = @storeId", new { storeId = storeId, newNumber = numOfOwners });
                    connection.Close();
                }

            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function removeOwnerNumerByOne in DB Store store id " + storeId);
                throw new ConnectionException();
            }
        }

        public void addownerNumerByOne(int storeId, int newNumber)
        {
            try
            {
                lock (connection)
                {
                    connection.Open();
                    //SqlConnection connection = Connector.getInstance().getSQLConnection();
                    connection.Execute("UPDATE [dbo].[Stores] SET numOfOwners = @newNumber WHERE storeId = @storeId", new { storeId = storeId, newNumber = newNumber });
                    connection.Close();

                }
            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function addOwnerNumerByOne in DB Store store id " + storeId);
                throw new ConnectionException();
            }
        }

        public int addStore(Store store)
        {
            int storeId = -1;
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                //connection.Open();
                lock (connection)
                {

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        ////////////////////////////////////////////////
                        string sql = "INSERT INTO [dbo].[Stores] (storeId, name,description,numOfOwners,active)" +
                                         " VALUES (@storeId, @name, @description,@numOfOwners,@active)";
                        storeId = store.getStoreID();
                        string name = store.getStoreName();
                        string description = store.getDescription();
                        int numOfOwners = store.getNumberOfOwners();
                        int active = 0;
                        if (store.isActive() == true)
                            active = 1;
                        LinkedList<PurchasePolicy> policies = store.getStorePolicyList();
                        foreach (PurchasePolicy p in policies)
                        {
                            addPolicyToDB(p, storeId);
                        }
                        connection.Execute(sql, new { storeId, name, description, numOfOwners, active }, transaction);


                        stores.AddFirst(store);

                        transaction.Commit();
                        connection.Close();
                        /////////////////////////
                        return store.getStoreID();


                    }
                }

            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function Addstore in DB Store store id " + storeId);
                throw new ConnectionException();
            }
        }

        public void addPolicyToDB(PurchasePolicy p, int storeID)
        {
            if (p is MinAmountPurchase)
                addMinPolicy(((MinAmountPurchase)p), storeID);
            else if (p is MaxAmountPurchase)
                addMaxPolicy(((MaxAmountPurchase)p), storeID);
            else if (p is TotalPricePolicy)
                addTotalPrice(((TotalPricePolicy)p), storeID);
            else if (p is ComplexPurchasePolicy)
                addComplexPolicy(((ComplexPurchasePolicy)p), storeID);
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
                // SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {

                    connection.Open();
                    LinkedList<Store> newStores = new LinkedList<Store>();
                    var StoreResult = connection.Query<StoreEntry>("SELECT * FROM [dbo].[Stores] WHERE storeId=@storeId ", new { storeId = storeId });
                    var StoreRoleResult = connection.Query<StoreRoleEntry>("SELECT * FROM [dbo].[StoreRoles] WHERE storeId=@storeId ", new { storeId = storeId });
                    var ContractResult = connection.Query<Contract>("SELECT * FROM [dbo].[Contracts] WHERE storeId = @storeId", new { storeId = storeId });
                    var pendingResult = connection.Query<string>("SELECT userName FROM [dbo].[PendingOwners] WHERE storeId = @storeId", new { storeId = storeId }).AsList();
                    var policyEntries = connection.Query<PolicyEntry>("SELECT * FROM [dbo].[PurchasePolicy] WHERE storeID=@storeId", new { storeID = storeId });
                    connection.Close();

                    StoreEntry se = StoreResult.ElementAt(0);
                    Store s = new Store(se.getStoreId(), se.getName(), se.getDescription());
                    foreach (Contract c in ContractResult)
                    {
                        s.getContracts().AddFirst(c);
                    }
                    foreach (string pending in pendingResult)
                    {
                        s.getPending().AddFirst(pending);
                    }

                    LinkedList<Product> lst = DBProduct.getInstance().getAllProducts();
                    foreach (Product p in lst)
                    {
                        if (p.getStoreID() == s.getStoreID())
                            s.addProduct(p);
                    }
                    s.setPolicyList(parsePolicy(policyEntries));
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
                            SubscribedUser appointedBy = DBSubscribedUser.getInstance().getSubscribedUserForInitStore(element.getAppointedBy());
                            SubscribedUser user = DBSubscribedUser.getInstance().getSubscribedUserForInitStore(element.getUserName());
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
                    LinkedList<DiscountComponent> discountList = DBDiscount.getInstance().getStoreDiscountsList(storeId);
                    s.setDiscountList(discountList);
                    return s;
                }

            }
            catch (Exception e)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function getStore in DB Store store id " + storeId);
                throw new ConnectionException();
            }


            ///////////////////////////////////////////////////

        }

        private LinkedList<PurchasePolicy> parsePolicy(IEnumerable<PolicyEntry> policyEntries)
        {
            int count = howManyComplex(policyEntries);
            if (count == 1)
            {
                return parseOneComplex(policyEntries);
            }
            else if (count == 2)
                return parseTwoComplex(policyEntries);
            else
                return parseNoComplex(policyEntries);

        }

        private LinkedList<PurchasePolicy> parseNoComplex(IEnumerable<PolicyEntry> policyEntries)
        {
            LinkedList<PurchasePolicy> policyList = new LinkedList<PurchasePolicy>();
            foreach (PolicyEntry p in policyEntries)
            {
                policyList.AddLast(parseRegular(p));
            }

                return policyList;

        }

        private LinkedList<PurchasePolicy> parseOneComplex(IEnumerable<PolicyEntry> policyEntries)
        {
            LinkedList<PurchasePolicy> policyList = new LinkedList<PurchasePolicy>();
            ComplexPurchasePolicy comp = null;
            int compPos = -1;
            int id1 = -1;
            int id2 = -1;
            for(int i =0; i<policyEntries.Count(); i++)
            {
                if (policyEntries.ElementAt(i).getType() == "complex")
                {
                    compPos = i;
                    id1 = policyEntries.ElementAt(i).getSubID1();
                    id2 = policyEntries.ElementAt(i).getSubID2();
                }
            }
            PurchasePolicy child1 = null;
            PurchasePolicy child2 = null;
            for (int i = 0; i < policyEntries.Count(); i++)
            {
                if (policyEntries.ElementAt(i).getPolicyID() == id1)
                {
                    child1 = parseRegular(policyEntries.ElementAt(i));   
                }
                else if (policyEntries.ElementAt(i).getPolicyID() == id2)
                {
                    child2 = parseRegular(policyEntries.ElementAt(i));
                }
                else
                {
                    policyList.AddLast(parseRegular(policyEntries.ElementAt(i)));
                }
            }
            comp = new ComplexPurchasePolicy(policyEntries.ElementAt(compPos).getCompType(), child1, child2);
            policyList.AddLast(comp);
            return policyList;


        }

        public PurchasePolicy parseRegular(PolicyEntry p)
        {
            if (p.getType() == "min")
            {
                MinAmountPurchase policy = new MinAmountPurchase(p.getAmount(), p.getPolicyID());
                return policy;
            }
            else if (p.getType() == "max")
            {
                MaxAmountPurchase policy = new MaxAmountPurchase(p.getAmount(), p.getPolicyID());
                return policy;
            }
            else
            {
                TotalPricePolicy policy = new TotalPricePolicy(p.getAmount(), p.getPolicyID());
                return policy;
            }
        }
        private LinkedList<PurchasePolicy> parseTwoComplex(IEnumerable<PolicyEntry> policyEntries)
        {
            LinkedList<PurchasePolicy> policyList = new LinkedList<PurchasePolicy>();
            ComplexPurchasePolicy compParent = null;
            ComplexPurchasePolicy compChild = null;
            int compParentPos = -1;
            int compChildPos = -1;
            int idRegularChild = -1;
            int id1Child = -1;
            int id2Child = -1;

            for (int i = 0; i < policyEntries.Count(); i++)
            {
                if (policyEntries.ElementAt(i).getType() == "complex" && policyEntries.ElementAt(i).getIsPartOfComp())
                {
                    compChildPos = i;
                    id1Child = policyEntries.ElementAt(i).getSubID1();
                    id2Child = policyEntries.ElementAt(i).getSubID2();
                }
                else if (policyEntries.ElementAt(i).getType() == "complex" && !policyEntries.ElementAt(i).getIsPartOfComp())
                {
                    compParentPos = i;  
                }
            }
            PurchasePolicy child1 = null;
            PurchasePolicy child2 = null;
            for (int i = 0; i < policyEntries.Count(); i++)
            {
                PolicyEntry p = policyEntries.ElementAt(i);
                if (p.getPolicyID() == id1Child)
                {
                    child1 = parseRegular(p);
                }
                else if (p.getPolicyID() == id2Child)
                {
                    child2 = parseRegular(p);
                }
                else if (p.getPolicyID() != id1Child & p.getPolicyID() != id2Child && p.getPolicyID() != compChildPos)
                    idRegularChild = i;
            }

            compChild = new ComplexPurchasePolicy(policyEntries.ElementAt(compChildPos).getCompType(), child1, child2, policyEntries.ElementAt(compChildPos).getPolicyID());
            PurchasePolicy regularChild = parseRegular(policyEntries.ElementAt(idRegularChild));
            compParent = new ComplexPurchasePolicy(policyEntries.ElementAt(compParentPos).getCompType(), compChild, regularChild, policyEntries.ElementAt(compParentPos).getPolicyID());

            policyList.AddLast(compParent);
            return policyList;


        }

        private int howManyComplex(IEnumerable<PolicyEntry> policyEntries)
        {
            int count = 0;
           for(int i=0; i<policyEntries.Count(); i++)
            {
                PolicyEntry p = policyEntries.ElementAt(i);
                if (p.getType()=="complex")
                    count++;
            }
            return count;
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
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                int storeId = store.getStoreID();
                lock (connection)
                {

                    connection.Open();
                    var affectedRows = connection.Execute("DELETE FROM Stores WHERE  storeId=@storeId ", new { storeId = storeId });
                    var affectedRowsPolicy = connection.Execute("DELETE FROM PurchasePolicy WHERE storeID=@storeId", new { storeId = storeId });
                    connection.Close();
                }



                /////
                stores.Remove(store);
                LinkedList<StoreRole> toRemove = new LinkedList<StoreRole>();
                foreach (StoreRole st in storeRole)
                {
                    if (st.getStore().getStoreID() == store.getStoreID())
                        //storeRole.Remove(st);
                        toRemove.AddFirst(st);
                }
                foreach(StoreRole st in toRemove)
                {
                    storeRole.Remove(st);
                }
            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function removeStore in DB Store store id " + store.getStoreID());
                throw new ConnectionException();
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
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {

                    connection.Open();
                    int idNum = connection.Query<int>("SELECT id FROM [dbo].[IDS] WHERE type=@type ", new { type = "store" }).First();
                    int next = idNum + 1;
                    connection.Execute("UPDATE [dbo].[IDS] SET id = @id WHERE type = @type", new { id = next, type = "store" });
                    nextStoreID = idNum;
                    connection.Close();
                    return idNum;

                }
            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function getNextStoreID in DB Store");
                throw new ConnectionException();
            }
        }
        internal int getNextPolicyID()
        {
            try
            {
                lock (connection)
                {
                    connection.Open();
                    int idNum = connection.Query<int>("SELECT id FROM [dbo].[IDS] WHERE type=@type ", new { type = "policy" }).First();
                    int next = idNum + 1;
                    connection.Execute("UPDATE [dbo].[IDS] SET id = @id WHERE type = @type", new { id = next, type = "policy" });
                    nextStoreID = idNum;
                    connection.Close();
                    return idNum;
                }
            }
            catch (Exception)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function getNextStoreID in DB Store");
                throw new ConnectionException();
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
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {

                    connection.Open();
                    int idNum = connection.Query<int>("SELECT id FROM [dbo].[IDS] WHERE type=@type ", new { type = "store" }).First();
                    connection.Close();
                    return idNum;

                }
            }
            catch (Exception e)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function getUpdatedId in DB Store store id ");
                throw new ConnectionException();
            }
        }
        public int getUpdatedPolicyID()
        {
            try
            {
                lock (connection)
                {
                    connection.Open();
                    int idNum = connection.Query<int>("SELECT id FROM [dbo].[IDS] WHERE type=@type ", new { type = "policy" }).First();
                    connection.Close();
                    return idNum;
                }
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
                // SqlConnection connection = Connector.getInstance().getSQLConnection();
                LinkedList<StoreRoleEntry> lst = new LinkedList<StoreRoleEntry>();
                lock (connection)
                {
                    connection.Open();
                    var StoreRoleResult = connection.Query<StoreRoleEntry>("SELECT * FROM [dbo].[StoreRoles] WHERE userName=@userName ", new { userName = userName });
                    connection.Close();


                    foreach (StoreRoleEntry element in StoreRoleResult)
                    {
                        lst.AddLast(element);
                    }
                    return lst;
                }

            }
            catch (Exception e)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function getRolesEntryByUserName in DB Store username" + userName);
                throw new ConnectionException();
            }
        }

        

        public void addPendingOwner(int storeId, string appointer, string pending)
        {

            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();

                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        string sql = "INSERT INTO [dbo].[PendingOwners] (storeId,userName)" +
                                           " VALUES (@storeId,@pending)";
                        connection.Execute(sql, new { storeId = storeId, pending = pending }, transaction);
                        Store s = getStore(storeId);
                        s.getPending().AddFirst(pending);
                        sql = "INSERT INTO [dbo].[Contracts] (storeId,userName,approvedBy)" +
                                          " VALUES (@storeId,@userName,@appointedBy)";
                        Contract c = new Contract(storeId, pending, appointer);
                        connection.Execute(sql, new { storeId = storeId, userName = pending, appointedBy = appointer }, transaction);
                        s = getStore(storeId);
                        s.getContracts().AddFirst(c);
                        transaction.Commit();
                        connection.Close();
                    }

                

            }
            catch (Exception e)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function addPendingOwner(" + storeId + "," + appointer + "," + pending + ") in DB Store ");
                throw new ConnectionException();
            }
        }

        ////////////////////////////////////////////////TODO eilonnnnn
        public void removePendingOwner(int storeId, string pending)
        {
            try
            {
                string sql = "DELETE FROM [dbo].[PendingOwners]" +
                           "WHERE storeId=@storeId AND userName = @pending";

                connection.Execute(sql, new { storeId = storeId, pending = pending }, ownerTrans);
                Store s = getStore(storeId);
                s.getPending().Remove(pending);
            }
            catch (Exception)
            {
                SystemLogger.getErrorLog().Error("Connection error in function removePendingOwner(" + storeId + "," + pending + ") in DB Store ");
                throw new ConnectionException();
            }
        }

        public void signContract(int storeId, string owner, string pending, bool saveTransaction)
        {
            try
            {

                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    if (ownerTrans==null)
                        connection.Open();
                    string sql = "INSERT INTO [dbo].[Contracts] (storeId,userName,approvedBy)" +
                               " VALUES (@storeId,@userName,@appointedBy)";

                    Contract c = new Contract(storeId, pending, owner);
                    if (saveTransaction)
                    {

                        try
                        {
                            connection.Execute(sql, new { storeId = storeId, userName = pending, appointedBy = owner }, ownerTrans);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                    else
                    {
                        connection.Execute(sql, new { storeId = storeId, userName = pending, appointedBy = owner });
                    }
                    Store s = getStore(storeId);
                    s.getContracts().AddFirst(c);
                    if (ownerTrans==null)
                         connection.Close();
                }

            }
            catch (Exception e)
            {
                if (ownerTrans == null)
                    connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function signContract(" + storeId + "," + owner + "," + pending + ") in DB Store ");
                throw new ConnectionException();
            }
        }

        public void removeAllUserContracts(int storeId, string userName)
        {
            try
            {
                Store s = getStore(storeId);
                LinkedList<Contract> cont = s.getContracts();
                for (int i = 0; i < cont.Count(); i++)
                {
                    if (cont.ElementAt(i).userName.Equals(userName))
                    {
                        cont.Remove(cont.ElementAt(i));
                    }
                }
                //SqlConnection connection = Connector.getInstance().getSQLConnection();

                string sql = "DELETE FROM [dbo].[Contracts]" +
                           "WHERE storeId=@storeId AND userName = @userName";
                connection.Execute(sql, new { storeId = storeId, userName = userName }, ownerTrans);


            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("Connection error in function removeAllUserContract(" + storeId + "," + userName + ") in DB Store ");
                throw new ConnectionException();
            }
        }

        public int getContractNum(int storeId, string userName)
        {
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {

                    connection.Open();
                    int count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM [dbo].[Contracts] WHERE storeId=@storeId AND userName = @userName", new { storeId = storeId, userName = userName });
                    connection.Close();
                    return count;

                }
            }
            catch (Exception e)
            {
                connection.Close();
                SystemLogger.getErrorLog().Error("Connection error in function getContractNum(" + storeId + "," + userName + ") in DB Store ");
                throw new ConnectionException();
            }


        }

        public bool hasContract(int storeId, string userName, string approved)
        {
            Store s = getStore(storeId);
            LinkedList<Contract> cont = s.getContracts();
            foreach (Contract c in cont)
            {
                if (c.userName.Equals(userName) && c.approvedBy.Equals(approved))
                    return true;
            }
            return false;
        }

        //public HashSet<string> getApproved(int storeId, string pending)
        //{
        //    HashSet<string> output;
        //    if (pendingOwners.TryGetValue(pending.getUsername(), out output))
        //    {
        //        return output;
        //    }
        //    throw new DoesntExistException("User is not a pending owner");
        //}


        public void addMinPolicy(MinAmountPurchase p, int storeID)
        {
            try
            {
                lock (connection)
                {
                    connection.Open();
                    int policyID = p.getPolicyID();
                    int amount = p.getAmount();
                    //  bool isPartOfComplex = false;
                    int isPartOfComplex = 0;
                    string type = "min";

                    string sql = "INSERT INTO [dbo].[PurchasePolicy] (storeID, policyID,type, amount,isPartOfComplex)" +
                                                            " VALUES (@storeID, @policyID,@type, @amount,@isPartOfComplex )";
                    connection.Execute(sql, new { storeID, policyID, type, amount, isPartOfComplex });
                    connection.Close();
                }
            }
            catch (Exception e) {
                connection.Close();
                throw new StoreException("cant add min policy");
            }
        }
        public void addMaxPolicy(MaxAmountPurchase p, int storeID)
        {
            try
            {
                lock (connection)
                {
                    connection.Open();
                    int policyID = p.getPolicyID();
                    int amount = p.getAmount();
                    //bool isPartOfComplex = false;
                    int isPartOfComplex = 0;
                    string type = "max";

                    string sql = "INSERT INTO [dbo].[PurchasePolicy] (storeID, policyID,type, amount,isPartOfComplex)" +
                                                            " VALUES (@storeID, @policyID,@type, @amount,@isPartOfComplex )";
                    connection.Execute(sql, new { storeID, policyID, type, amount, isPartOfComplex });
                    connection.Close();
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("cant add min policy");
            }
        }
        public void addTotalPrice(TotalPricePolicy p, int storeID)
        {
            try
            {
                lock (connection)
                {
                    connection.Open();
                    int policyID = p.getPolicyID();
                    int amount = p.getAmount();
                    int isPartOfComplex = 0;
                    string type = "total";

                    string sql = "INSERT INTO [dbo].[PurchasePolicy] (storeID, policyID,type, amount,isPartOfComplex)" +
                                                           " VALUES (@storeID, @policyID,@type, @amount,@isPartOfComplex )";
                    connection.Execute(sql, new { storeID, policyID, type, amount, isPartOfComplex });
                    connection.Close();
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("cant add min policy");
            }
        }
        public void addComplexPolicy(ComplexPurchasePolicy p, int storeID)
        {
            try
            {
                lock (connection)
                {
                    connection.Open();
                    int policyID = p.getPolicyID();
                    int isPartOfComplex = 0;
                    string type = "complex";
                    int subtype1 = p.getFirstChildID();
                    int subtype2 = p.getSecondChildID();
                    string compType = p.getCompType();
                    int isPartOfComplexChild = 1;
                    string sql = "INSERT INTO [dbo].[PurchasePolicy] (storeID, policyID,type,isPartOfComplex, subtypeID1, subtypeID2, compType )" +
                                                            " VALUES (@storeID,@policyID,@type,@isPartOfComplex,@subtype1,@subtype2,@compType )";
                    connection.Execute(sql, new { storeID, policyID, type, isPartOfComplex, subtype1, subtype2, compType });
                    string sql1 = "UPDATE [dbo].[PurchasePolicy] SET isPartOfComplex = @isPartOfComplexChild WHERE storeID =@storeID AND policyID=@subtype1";
                    connection.Execute(sql1, new { storeID, subtype1, isPartOfComplexChild });
                    string sql2 = "UPDATE [dbo].[PurchasePolicy] SET isPartOfComplex= @isPartOfComplexChild WHERE storeID =@storeID AND policyID=@subtype2";
                    connection.Execute(sql2, new { storeID, subtype2, isPartOfComplexChild });
                    connection.Close();
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("cant add min policy");
            }
        }

        public void setPolicy(PurchasePolicy p, int storeID, int newAmount)
        {
            try
            {
                lock (connection)
                {
                    connection.Open();
                    int policyID = p.getPolicyID();
                    string sql = "UPDATE[dbo].[PurchasePolicy] SET amount=@newAmount WHERE storeID=@storeID AND policyID=@policyID";
                    connection.Execute(sql, new { newAmount, storeID, policyID });
                    connection.Close();
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("cant add min policy");
            }
        }

        public void removePolicy(PurchasePolicy p, int storeID)
        {
            try
            {
                lock (connection)
                {
                    connection.Open();
                    int policyID = p.getPolicyID();
                    string sql = "DELETE FROM PurchasePolicy WHERE storeID=@storeID AND policyID=@policyID";
                    connection.Execute(sql, new { storeID, policyID });
                    connection.Close();
                }
            }
            catch (Exception)
            {
                connection.Close();
                throw new StoreException("cant add min policy");
            }
            if (p is ComplexPurchasePolicy)
            {
                PurchasePolicy p1 = ((ComplexPurchasePolicy)p).getFirstPolicyChild();
                PurchasePolicy p2 = ((ComplexPurchasePolicy)p).getSecondPolicyChild();
                removePolicy(p1, storeID);
                removePolicy(p2, storeID);
            }
        }

        public void signAndAddOwner(int storeId, string owner, string pending, StoreRole toAdd)
        {
            //SqlConnection connection = Connector.getInstance().getSQLConnection();
            lock (connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ownerTrans = transaction;
                        signContract(storeId, owner, pending, true);
                        removePendingOwner(storeId, pending);
                        addStoreRole(toAdd);
                        transaction.Commit();
                        connection.Close();
                    }
                    catch (Exception e)
                    {
                        connection.Close();
                        ownerTrans = null;
                        throw e;
                    }
                }
                ownerTrans = null;
            }

        }

        public void declineContract(int storeId, string pending)
        {
            //SqlConnection connection = Connector.getInstance().getSQLConnection();
            lock (connection)
            {

                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ownerTrans = transaction;
                        removePendingOwner(storeId, pending);
                        removeAllUserContracts(storeId, pending);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {

                        ownerTrans = null;
                        throw e;
                    }
                }
                connection.Close();
                ownerTrans = null;
            }

        }
    }
}
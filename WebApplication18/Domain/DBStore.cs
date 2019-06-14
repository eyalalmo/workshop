using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication18.DAL;
using Dapper;
using WebApplication18.Domain;
using System.Data.SqlClient;

namespace workshop192.Domain
{
    public class DBStore:Connector
    {
        private SqlTransaction ownerTrans = null;
        public static DBStore instance;
        public LinkedList<Store> stores;
        public LinkedList<StoreRole> storeRole;

        public static int nextStoreID = 0;

        private DBStore()
        {
            if (MarketSystem.testsMode == false)
            {
                storeRole = new LinkedList<StoreRole>();
                stores = new LinkedList<Store>();
                //nextStoreID = getUpdatedId();
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
                    connection.Execute("UPDATE [dbo].[IDS] SET id = 0 WHERE type = 'store'");
                    connection.Close();
                }
                //connection.Close();
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
           
            //SqlConnection connection = Connector.getInstance().getSQLConnection();
            lock (connection)
            {

                using (connection)
                {
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
                        }
                        connection.Close();

                    }
                    catch (Exception e)
                    {

                        if (ownerTrans == null)
                        {
                            transaction.Dispose();
                        }
                        throw new StoreException("cant add store roll");
                    }
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
                    using (connection)
                    {
                        connection.Open();

                        //SqlConnection connection = Connector.getInstance().getSQLConnection();
                        connection.Execute("UPDATE [dbo].[Stores] SET numOfOwners = @newNumber WHERE storeId = @storeId", new { storeId = storeId, newNumber = numOfOwners });
                        //connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //connection.Close();
            }
        }

        public void addownerNumerByOne(int storeId, int newNumber)
        {
            try
            {
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        //SqlConnection connection = Connector.getInstance().getSQLConnection();
                        connection.Execute("UPDATE [dbo].[Stores] SET numOfOwners = @newNumber WHERE storeId = @storeId", new { storeId = storeId, newNumber = newNumber });
                        //connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //connection.Close();
            }
        }

        public void setMinPurchasePolicy(int storeId, int minPurchasePolicy)
        {
            try
            {
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        //SqlConnection connection = Connector.getInstance().getSQLConnection();
                        connection.Execute("UPDATE [dbo].[Stores] SET minPurchasePolicy = @minPurchasePolicy WHERE storeId = @storeId", new { storeId = storeId, minPurchasePolicy = minPurchasePolicy });
                        //connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //connection.Close();
            }
        }

        public int addStore(Store store)
        {
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                //connection.Open();
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {


                                ////////////////////////////////////////////////
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

                                connection.Execute(sql, new { storeId, name, description, numOfOwners, active, minPurchasePolicy, maxPurchasePolicy }, transaction);

                                //connection.Close();
                                stores.AddFirst(store);

                                transaction.Commit();
                                /////////////////////////
                                return store.getStoreID();
                            }
                            catch (Exception)
                            {
                                //connection.Close();
                                transaction.Rollback();
                                throw new StoreException("faild to add store");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new ConnectionException("Bad connection to database");
            }
        }

        public void setMaxPurchasePolicy(int storeId, int maxPurchasePolicy)
        {
            try
            {
                // SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        connection.Execute("UPDATE [dbo].[Stores] SET maxPurchasePolicy = @maxPurchasePolicy WHERE storeId = @storeId", new { storeId = storeId, maxPurchasePolicy = maxPurchasePolicy });
                        //connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //connection.Close();
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
                // SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        LinkedList<Store> newStores = new LinkedList<Store>();
                        var StoreResult = connection.Query<StoreEntry>("SELECT * FROM [dbo].[Stores] WHERE storeId=@storeId ", new { storeId = storeId });
                        var StoreRoleResult = connection.Query<StoreRoleEntry>("SELECT * FROM [dbo].[StoreRoles] WHERE storeId=@storeId ", new { storeId = storeId });
                        var ContractResult = connection.Query<Contract>("SELECT * FROM [dbo].[Contracts] WHERE storeId = @storeId", new { storeId = storeId });
                        var pendingResult = connection.Query<string>("SELECT userName FROM [dbo].[PendingOwners] WHERE storeId = @storeId", new { storeId = storeId }).AsList();
                        //connection.Close();

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
            }
            catch (Exception e)
            {
                //connection.Close();
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
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                int storeId = store.getStoreID();
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        var affectedRows = connection.Execute("DELETE FROM Stores WHERE  storeId=@storeId ", new { hash = storeId });
                    }
                }
                //connection.Close();


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
                //connection.Close();
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
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        int idNum = connection.Query<int>("SELECT id FROM [dbo].[IDS] WHERE type=@type ", new { type = "store" }).First();
                        int next = idNum + 1;
                        connection.Execute("UPDATE [dbo].[IDS] SET id = @id WHERE type = @type", new { id = next, type = "store" });
                        nextStoreID = idNum;
                        //connection.Close();
                        return idNum;
                    }
                }
            }
            catch (Exception)
            {
                //connection.Close();
                throw new ConnectionException("Database error");
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
                    using (connection)
                    {
                        connection.Open();
                        int idNum = connection.Query<int>("SELECT id FROM [dbo].[IDS] WHERE type=@type ", new { type = "store" }).First();
                        //connection.Close();
                        return idNum;
                    }
                }
            }
            catch (Exception e)
            {
                //connection.Close();
                throw new ConnectionException("Database error");
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
                    using (connection)
                    {
                        connection.Open();
                        var StoreRoleResult = connection.Query<StoreRoleEntry>("SELECT * FROM [dbo].[StoreRoles] WHERE userName=@userName ", new { userName = userName });
                        //connection.Close();


                        foreach (StoreRoleEntry element in StoreRoleResult)
                        {
                            lst.AddLast(element);
                        }
                        return lst;
                    }
                }
            }
            catch (Exception e)
            {
                //connection.Close();
                throw new ConnectionException("Database error - Getting store roles of user " + userName);
            }
        }
      
        public void addPendingOwner(int storeId, string appointer, string pending)
        {

            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    using (connection) {
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
                    }
                }
            }

            }
            catch (Exception e)
            { 
                throw new ConnectionException("Bad connection to database while adding a pending owner"); 
            }
        }


        public void removePendingOwner(int storeId, string pending)
        {
            try
            {
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        //SqlConnection connection = Connector.getInstance().getSQLConnection();
                        string sql = "DELETE FROM [dbo].[PendingOwners]" +
                                   "WHERE storeId=@storeId AND userName = @pending";

                        connection.Execute(sql, new { storeId = storeId, pending = pending }, ownerTrans);
                        Store s = getStore(storeId);
                        s.getPending().Remove(pending);
                        //connection.Close();
                    }
                }
            }
            catch (Exception)
            {
                //connection.Close();
                throw new StoreException("cant remove pending owner");
            }
        }

        public void signContract(int storeId, string owner, string pending,bool saveTransaction)
        {
            try
            {

                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    using (connection)
                    {
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
                        //connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //connection.Close();
                throw new StoreException("Error in signing a contract: " + e.Message.ToString());
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
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        string sql = "DELETE FROM [dbo].[Contracts]" +
                                   "WHERE storeId=@storeId AND userName = @userName";
                        connection.Execute(sql, new { storeId = storeId, userName = userName }, ownerTrans);
                        //connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //connection.Close();
                throw new StoreException("Error in declining a contract: " + e.Message.ToString());
            }

}

        public int getContractNum(int storeId,  string userName)
        {
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        int count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM [dbo].[Contracts] WHERE storeId=@storeId AND userName = @userName", new { storeId = storeId, userName = userName });
                        //connection.Close();
                        return count;
                    }
                }
            }
            catch(Exception e)
            {
                throw new StoreException("cant get contact");
            }
            

        }

        public bool hasContract(int storeId,string userName,string approved)
        {
            Store s = getStore(storeId);
            LinkedList < Contract > cont = s.getContracts();
            foreach(Contract c in cont)
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

        public void signAndAddOwner(int storeId, string owner, string pending,StoreRole toAdd)
        {
            //SqlConnection connection = Connector.getInstance().getSQLConnection();
            lock (connection)
            {

                using (connection)
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
                        }
                        catch (Exception e)
                        {

                            ownerTrans = null;
                            throw e;
                        }
                    }
                    ownerTrans = null;
                }
            }
        }

        public void declineContract(int storeId, string pending)
        {
            //SqlConnection connection = Connector.getInstance().getSQLConnection();
            lock (connection)
            {
                using (connection)
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
                    ownerTrans = null;
                }
            }
        }
    }
}
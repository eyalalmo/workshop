using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class StoreDatabase
    {
        public static StoreDatabase instance;
        private LinkedList<Store> stores;
        private LinkedList<StoreRole> storeRole;
        private int nextStoreID;

        public static StoreDatabase getInstane()
        {
            if (instance == null)
                instance = new StoreDatabase();
            return instance;
        }

        public StoreDatabase()
        {
            stores = new LinkedList<Store>();
            storeRole = new LinkedList<StoreRole>();
            nextStoreID = 0;
        }

        public void removeStoreRole(StoreRole sr)
        {
            storeRole.Remove(sr);
        }

        public void removeStoreRole(Store store, SubscribedUser user)
        {
            StoreRole sr = getStoreRole(store, user);
            if(sr ! = null)
                storeRole.Remove(sr);
        
        }

       
        public StoreRole getStoreRole(Store store, SubscribedUser user)
        {
           foreach(StoreRole st in storeRole)
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
            storeRole.AddFirst(sr);
        }
        public void addStore(Store store)
        {
            stores.AddFirst(store);
        } 

        public Store getStore(int storeID)
        {
            foreach(Store s in stores)
            {
                if (s.getStoreID() == storeID)
                    return s;
            }
            return null;
        }
        public String removeStore(Store store)
        {
            if (!stores.Contains(store))
            {
                return " - no such store";
            }
            stores.Remove(store);
            foreach(StoreRole st in storeRole)
            {
                if (st.getStore().getStoreID() == store.getStoreID())
                    storeRole.Remove(st);
            }
            return "";
        }

        public void closeStore(Store s)
        {
            if (stores.Contains(s))
            {
                s.changeStatus();
                ////////////////////////////////notify all
            }

              


        }

        public LinkedList<Store> getAllStores()
        {
            return stores;
        }

        public int getNextStoreID()
        {
            int id = nextStoreID;
            nextStoreID++;
            return id;
        }

        public void addStore(Store s)
        {
            stores.AddFirst(s);
        }

            getStore - id / null
    }
}

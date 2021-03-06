﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class DBStore
    {
        private static DBStore instance;
        private LinkedList<Store> stores;
        private LinkedList<StoreRole> storeRole;
        private static int nextStoreID=0;

        public static DBStore getInstance()
        {
            if (instance == null) 
                instance = new DBStore();

            return instance;
        }

        public void DBinit()
        {
            stores = new LinkedList<Store>();
            storeRole = new LinkedList<StoreRole>();
            nextStoreID = 0;
        }
        private DBStore()
        {
            stores = new LinkedList<Store>();
            storeRole = new LinkedList<StoreRole>();
           // nextStoreID = 0;
        }
        public void init()
        {
            stores = new LinkedList<Store>();
            storeRole = new LinkedList<StoreRole>();
            nextStoreID = 0;
        }

        public void cleanDB()
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
            if (sr != null)
                storeRole.Remove(sr);

        }

        public StoreRole getStoreRole(Store store, SubscribedUser user)
        {
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
            storeRole.AddFirst(sr);
        }
        public int addStore(Store store)
        {

            stores.AddFirst(store);
            return store.getStoreID();
        }

        public Store getStore(int storeID)
        {
            foreach (Store s in stores)
            {
                if (s.getStoreID() == storeID)
                    return s;
            }
            return null;
        }
        public void removeStore(Store store)
        {
            if (!stores.Contains(store))
            {
                throw new DoesntExistException("store " + store.getStoreName() + " doesn't exist");
            }
            stores.Remove(store);
            foreach (StoreRole st in storeRole)
            {
                if (st.getStore().getStoreID() == store.getStoreID())
                    storeRole.Remove(st);
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

        public LinkedList<Store> getAllStores()
        {
            return stores;
        }

        public static int getNextStoreID()
        {
            int id = nextStoreID;
            nextStoreID++;
            return id;
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
    }
}

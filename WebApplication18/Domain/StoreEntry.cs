using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace workshop192.Domain
{
    public class StoreEntry
    {
        public int storeId;
        public string name;
        public string description;
        public int numOfOwners;
        public int active;
        //public int minPurchasePolicy;
        //public int maxPurchasePolicy;
        public StoreEntry(int storeId,string name, string description, int numOfOwners, int active)
        {
            this.storeId = storeId;
            this.name = name;
            this.description = description;
            this.numOfOwners = numOfOwners;
            this.active= active;
            //this.minPurchasePolicy= minPurchasePolicy;
            //this.maxPurchasePolicy= maxPurchasePolicy;

    }
        public int getStoreId()
        {
            return storeId;
        }
        public string getName()
        {
            return name;
        }
        public string getDescription()
        {
            return description;
        }
        public int getNumOfOwners()
        {
            return numOfOwners;
        }
        public int getActive()
        {
            return active;
        }
        /*
        public int getMinPurchasePolicy()
        {
            return minPurchasePolicy;
        }
        public int getMaxPurchasePolicy()
        {
            return maxPurchasePolicy;
        }
        */




    }
}
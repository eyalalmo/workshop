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

        public StoreEntry(int storeId,string name, string description, int numOfOwners, int active)
        {
            this.storeId = storeId;
            this.name = name;
            this.description = description;
            this.numOfOwners = numOfOwners;
            this.active= active;
            

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
  




    }
}
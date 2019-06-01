using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication18.Domain
{
    public class BasketCartEntry
    {
        private string username;
        private int storeID;

        public BasketCartEntry(string username, int storeID)
        {
            this.username = username;
            this.storeID = storeID;
        }

        public string getUsername()
        {
            return username;
        }

        public int getStoreID()
        {
            return storeID;
        }
    }
}
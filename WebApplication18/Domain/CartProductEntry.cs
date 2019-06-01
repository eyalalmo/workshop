﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication18.Domain
{
    public class CartProductEntry
    {
        private int productID;
        private int storeID;
        private int amount;

        public CartProductEntry(int productID, int storeID, int amount)
        {
            this.productID = productID;
            this.storeID = storeID;
            this.amount = amount;
        }
        public int getProductID() { return this.productID; }
        public int getStoreID() { return this.storeID; }
        public int getAmount() { return this.amount; }

    }
}
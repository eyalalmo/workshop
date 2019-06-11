﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication18.Domain
{
    public class DiscountEntry
    {
        private int id;
        private string type;
        private string reliantType;
        private string visibleType;
        private int productId;
        private int numOfProducts;
        private int totalAmount;
        private DiscountComponentEntry component;
        private int isPartOfComplex;

        public DiscountEntry(int id, string type,int isPartOfComplex, string reliantType, string visibleType, int productId, int numOfProducts, int totalAmount)
        {
            this.id = id;
            this.type = type;
            this.isPartOfComplex = isPartOfComplex;
            this.reliantType = reliantType;
            this.visibleType = visibleType;
            this.productId = productId;
            this.numOfProducts = numOfProducts;
            this.totalAmount = totalAmount;
        }
        public string getType()
        {
            return this.type;
        }
        public string getReliantType()
        {
            return this.reliantType;
        }
        public string getVisibleType()
        {
            return this.visibleType;
        }
        public int getProductId()
        {
            return this.productId;
        }
        public int getNumOfProducts()
        {
            return this.numOfProducts;
        }
        public int getTotalAmount()
        {
            return this.totalAmount;
        }
        public void setDiscountComponentEntry(DiscountComponentEntry component)
        {
            this.component = component;
        }
        public int getId()
        {
            return this.id;
        }
        public int getIsPartOfComplex()
        {
            return this.isPartOfComplex;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication18.Domain
{
    public class DiscountEntry
    {
        private string type;
        private string reliantType;
        private string visibleType;
        int productId;
        int numOfProducts;
        int totalAmount;

        public DiscountEntry(string type, string reliantType, string visibleType, int productId, int numOfProducts, int totalAmount)
        {
            this.type = type;
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
    }
}
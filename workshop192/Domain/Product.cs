using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class Product
    {

        //represents product in store
        private int productID;
        private string productName;
        private string productCategory;
        private int price;
        private int storeID;
        private int rank;
        private int quantityLeft;
        private Discount discount;

       
        public Product(string productName, string productCategory, int price, int rank, int quantityLeft, int storeID)
        {
            this.productID = DBProduct.nextProductID;
            this.productName = productName;
            this.productCategory = productCategory;
            this.price = price;
            this.rank = rank;
            this.quantityLeft = quantityLeft;
            this.storeID = storeID;
            this.discount = null;

        }
          
        private bool checkValidInfo(int price, int rank, int quantityLeft) ////// מיותר
        {
            if (rank < 1 || rank > 5)
                return false;
            if (price <= 0)
                return false;
            if (quantityLeft < 0)
                return false;
            return true;
        }

        public int getActualPrice()
        {
            return this.price;
        }

        public int getQuantityLeft()
        {
            throw new NotImplementedException();
        }


        public void setQuantityLeft(int quantity)
        {
            this.quantityLeft = quantity;
        }

        public void addQuantityLeft( int amount)
        {
           quantityLeft = quantityLeft + amount;
        }
        public void decQuantityLeft(int amount)
        {
            quantityLeft = quantityLeft - amount;
        }
        public int getProductID()
        {
            return productID;
        }
        public String getProductName()
        {
            return this.productName;
        }
        public String getProductCategory()
        {
            return productCategory;
        }
        public int getPrice()
        {
            return price;
        }

        public int getRank()
        {
            return rank;
        }


        public void setProductID(int id)
        {
            this.productID = id;
        }
        public void setProductName(String productName)
        {
            this.productName = productName;
        }
        public void setProductCategory(String category)
        {
            this.productCategory = category;
        }
        public void setPrice(int price)
        {
            this.price = price;
        }

        public void setRank(int rank)
        {
            this.rank = rank;
        }
        public int getStoreID()
        {
            return this.storeID;
        }

    }
}

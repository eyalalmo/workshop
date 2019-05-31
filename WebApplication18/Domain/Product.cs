using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace workshop192.Domain
{
    public class Product
    {
        //represents product in store
        public int productID;
        public string productName;
        public string productCategory;
        public int price;
        private Store store;
        public int rank;
        public int quantityLeft;
        public int storeID;
        [JsonIgnore]
        public VisibleDiscount discount;
        [JsonIgnore]
        public ReliantDiscount sameProductDiscount;


        public Product(string productName, string productCategory, int price, int rank, int quantityLeft, Store store)
        {
            this.productID = DBProduct.getNextProductID();
            this.productName = productName;
            this.productCategory = productCategory;
            this.price = price;
            this.rank = rank;
            this.storeID = store.getStoreID();
            this.quantityLeft = quantityLeft;
            this.store = store;
            //this.numberOfRanking = 0;
            this.discount = null;

        }


        public double getActualPrice(int amountinBasket)
        {
            double actualPrice = price;
            if (discount != null && !discount.getIsPartOfComplex())
            {
                actualPrice = price * (1 - discount.getPercentage());
            }
            if (sameProductDiscount != null && !sameProductDiscount.getIsPartOfComplex()) { 
                if (sameProductDiscount.getMinNumOfProducts() <= amountinBasket)
                 {
                            actualPrice = price * (1 - sameProductDiscount.getPercentage());
                       
                 }
            }
            if (sameProductDiscount != null && sameProductDiscount.getIsPartOfComplex() && sameProductDiscount.getComplexCondition())
            {
                if (sameProductDiscount.getMinNumOfProducts() <= amountinBasket)
                {
                    actualPrice = price * (1 - sameProductDiscount.getPercentage());

                }
            }
            if (discount != null && discount.getIsPartOfComplex() &&discount.getComplexCondition())
            {
                actualPrice = price * (1 - discount.getPercentage());
            }
            return actualPrice;
        }

        public double getActualPrice()
        {
            double actualPrice = price;
            if (discount != null)
            {
                actualPrice = price * (1 - discount.getPercentage());
            }

            return actualPrice;
        }

        public void setReliantDiscountSameProduct(ReliantDiscount d)
        {
            this.sameProductDiscount = d;
        }
        public int getQuantityLeft()
        {
            return this.quantityLeft;
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

        public void setDiscount(VisibleDiscount discount)
        {
            this.discount = discount;
            store.addDiscount(discount);
        }

        public void removeDiscount()
        {
            if (discount == null)
                throw new DoesntExistException("Error: Discount does not exist so it cannot be removed");
            else
                discount = null;
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

        public Store getStore()
        {
            return store;
        }
    }
}

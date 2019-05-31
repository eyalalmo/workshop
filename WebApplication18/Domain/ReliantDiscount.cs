using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class ReliantDiscount : Discount
    {
        public enum reliantType { sameProduct, totalAmount }
        private reliantType type;
        int numOfProducts;
        int totalAmount;
        Product product;
        public bool discountPartOfComplex;

        // Dictionary<Product, int> products;

        public ReliantDiscount(double percentage, String duration, int numOfProducts, Product product) : base(percentage, duration)
        {
            this.numOfProducts = numOfProducts;
            this.type = reliantType.sameProduct;
            this.product = product;
            this.discountPartOfComplex = false;
        }

        public ReliantDiscount(double percentage, String duration, int amount) : base(percentage, duration)
        {
            this.totalAmount = amount;
            this.type = reliantType.totalAmount;
        }
        
        public override bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            if (type == reliantType.sameProduct)
            {
                if (productList.ContainsKey(product))
                {
                    if (productList[product] >= numOfProducts)
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else if (type == reliantType.totalAmount)
            {
                double sum = 0;
                foreach (KeyValuePair<Product, int> entry in productList)
                {
                    Product p = entry.Key;
                    double actualPrice = productsActualPrice[p];
                    sum += (entry.Key.getPrice() * actualPrice);
                }
                if (sum < totalAmount)
                    return false;
                return true;

            }

            return false;
        }
  

        public override Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            if (type == reliantType.sameProduct)
            {

                productsActualPrice[product] = productsActualPrice[product] * (1 - percentage);

            }
            if (type == reliantType.totalAmount)
            {
                //foreach (KeyValuePair<Product, int> entry in products)
                //{
                //    productsActualPrice[entry.Key] = productsActualPrice[product] * (1 - percentage);
                //}
            }
            return productsActualPrice;
        }
        public override string description()
        {
            if (type == reliantType.sameProduct)
            {
                return "Quantity of product " + product.getProductName() + " ID: " + product.getProductID() +" is at least " + numOfProducts + " Discount: "+percentage*100+"%";
            }
            if(type == reliantType.totalAmount)
            {
                return "Total cart price over $" + totalAmount + " Discount: "+percentage*100+"%";
            }
            return "";
        }

        public override string getDiscountType()
        {
            return "Reliant Discount";
        }
        public Product getProduct()
        {
            return this.product;
        }
        public int getMinNumOfProducts()
        {
            return this.numOfProducts;
        }

        public int getTotalAmount()
        {
            return this.totalAmount;
        }
        public bool isTotalAmountDiscount()
        {
            if (type == reliantType.totalAmount)
                return true;
            return false;
        }
        public bool isSameProductDiscount()
        {
            if (type == reliantType.sameProduct)
                return true;
            return false;
        }

    }
}

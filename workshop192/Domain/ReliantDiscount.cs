using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class ReliantDiscount : Discount
    {
        public enum reliantType {sameProduct,totalAmount}
        private double percentage;
        private String duration;
        private reliantType type;
        int numOfProducts;
        int totalAmount;
        Product product;
        Dictionary<Product, int> products;

        public ReliantDiscount(int id, double percentage, String duration, int numOfProducts, string type, Product product) : base(id, percentage, duration)
        {
          
                this.numOfProducts = numOfProducts;
                this.type = reliantType.sameProduct;
                this.product = product;
                   }
        public ReliantDiscount(int id, double percentage, String duration, int amount, string type) : base( percentage, duration, id)
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
           else if(type == reliantType.totalAmount)
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

            return false ;
        }

        public override Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            if(type == reliantType.sameProduct)
            {
              
                productsActualPrice[product] = productsActualPrice[product] * (1 - percentage);
                
            }
            if (type == reliantType.totalAmount)
            {
                foreach (KeyValuePair<Product, int> entry in products)
                {
                    productsActualPrice[entry.Key] = productsActualPrice[product] * (1 - percentage);
                }
            }
            return productsActualPrice;
        }
    }
}

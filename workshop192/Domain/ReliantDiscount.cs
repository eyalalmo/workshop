using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class ReliantDiscount : Discount
    {
        public enum reliantType { sameProduct, multiProducts, totalAmount }
        private double percentage;
        private String duration;
        private reliantType type;
        int amount;
        int totalAmount;
        Product product;
        Dictionary<Product, int> products;

        public ReliantDiscount(double percentage, String duration, int amount, string type, Product product) : base(percentage, duration)
        {
            if (type == "sameProduct")
            {
                this.amount = amount;
                this.type = reliantType.sameProduct;
                this.product = product;
            }
            else if (type == "totalAmount")
            {
                this.totalAmount = amount;
                this.type = reliantType.totalAmount;
            }

        }

        public ReliantDiscount(double percentage, String duration, Dictionary<Product, int> products) : base(percentage, duration)
        {
            this.type = reliantType.multiProducts;
            this.products = products;
        }



        public override bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            if (type == reliantType.sameProduct)
            {
                if (productList.ContainsKey(product))
                {
                    if (productList[product] >= amount)
                    {
                        return true;
                    }
                }
            }
            else if (type == reliantType.multiProducts)
            {
                bool cond = true;
                foreach (KeyValuePair<Product, int> entry in products)
                {
                    if (!productList.ContainsKey(entry.Key) || products[entry.Key] > productList[entry.Key])
                    {
                        cond = false;
                        break;
                    }

                }
                if (cond)
                {
                    return true;
                }
            }

            return false;
        }

        public override Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            if (type == reliantType.sameProduct)
            {
                if (productList.ContainsKey(product))
                {
                    if (productList[product] >= amount)
                    {
                        productsActualPrice[product] = productsActualPrice[product] * (1 - percentage);
                    }
                }
            }
            if (type == reliantType.multiProducts)
            {
                bool cond = true;
                foreach (KeyValuePair<Product, int> entry in products)
                {
                    if (!productList.ContainsKey(entry.Key) || products[entry.Key] > productList[entry.Key])
                    {
                        cond = false;
                        break;
                    }

                }
                if (cond)
                {
                    foreach (KeyValuePair<Product, int> entry in products)
                    {
                        productsActualPrice[entry.Key] = productsActualPrice[product] * (1 - percentage);
                    }
                }
            }

            return productsActualPrice;

        }
    }
}
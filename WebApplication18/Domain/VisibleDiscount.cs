using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class VisibleDiscount : Discount
    {
        public visibleType type;
        public Product product;
        public enum visibleType { productVisibleDiscount, storeVisibleDiscount }
        public VisibleDiscount(double percentage, string duration, string t): base (percentage, duration) {
            if (t.Equals("StoreVisibleDiscount"))
            {
                type = visibleType.storeVisibleDiscount;   
            }
            else if (t.Equals("ProductVisibleDiscount"))
            {
                type = visibleType.productVisibleDiscount;
            }
            else
            {
                throw new IllegalNameException();
            }
        }

        public override Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            // this func only for store visible discount
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                productsActualPrice[entry.Key] = productsActualPrice[entry.Key] * (1 -this.getPercentage());
            }
            return productsActualPrice;
        }

        public override bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            return true;
        }
        public void setProduct(Product p)
        {
            this.product = p;
        }
        public override string description()
        {
            if (type == visibleType.productVisibleDiscount)
            {
               return "Product " + product.getProductName() + " ID: " + product.getProductID();
            }
            else
            {
                return "Entire Store Discount";
            }
        }

        public override string getDiscountType()
        {
            return "Visible Discount";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class InvisibleDiscount : Discount
    {
        private string coupon;
        
        public InvisibleDiscount(double percentage, string coupon, string duration):base(percentage, duration)
        {
            this.coupon = coupon;
        }
       
        public override bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            throw new NotImplementedException();
        }
        public bool checkCoupon(string c)
        {
            if (c == coupon)
                return true;
            return false;
        }
        public override Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                productsActualPrice[entry.Key] = productsActualPrice[entry.Key] * (1 - this.getPercentage());
            }
            return productsActualPrice;
        }

        internal string getCoupon()
        {
            return this.coupon;
        }
    }
}

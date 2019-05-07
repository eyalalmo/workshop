using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class VisibleDiscount : Discount
    {

        public VisibleDiscount(double percentage, string duration): base (percentage, duration) { }

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
    }
}

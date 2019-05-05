using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{

    public abstract class DiscountComponent
    {
        int id;
        
        
        public abstract bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice);
        public abstract  Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice);

        public DiscountComponent(int id)
        {
            this.id = id;
        }
        public int getId()
        {
            return this.id;
        }

    }
}

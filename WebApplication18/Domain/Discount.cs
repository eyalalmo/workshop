using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public abstract class Discount : DiscountComponent
    {

        internal Discount(int id,bool isPartOfComplex, double percentage, string duration, int storeId) : base(id, percentage, duration, storeId, isPartOfComplex)
        {
        }

        public Discount(double percentage, string duration, int storeId): base(percentage, duration, storeId)
        {
        }

        public abstract Product getProduct();
        public abstract void removeProduct();
        public override void setComplexCondition(bool complexCondition, Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            if (complexCondition)
                this.complexCondition = checkCondition(productList, productsActualPrice);
            this.complexCondition = complexCondition;
        }
     
    }
}

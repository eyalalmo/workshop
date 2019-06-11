using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public abstract class Discount : DiscountComponent
    {
        protected bool isPartOfComplex;

        internal Discount(int id,bool isPartOfComplex, double percentage, string duration, int storeId) : base(id, percentage, duration, storeId)
        {
            this.isPartOfComplex = isPartOfComplex;
        }

        public Discount(double percentage, string duration, int storeId): base(percentage, duration, storeId)
        {
            this.isPartOfComplex = false;
        }

        public bool getIsPartOfComplex() {
            return this.isPartOfComplex;
        }
        public void setIsPartOfComplex(bool isPartOfComplex)
        {
            this.isPartOfComplex = isPartOfComplex;
        }
        public override void setComplexCondition(bool complexCondition, Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            if (complexCondition)
                this.complexCondition = checkCondition(productList, productsActualPrice);
            this.complexCondition = complexCondition;
        }
     
    }
}

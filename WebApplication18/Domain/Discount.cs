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
        private int id;

        internal Discount(int id, double percentage, string duration) : base(percentage, duration)
        {
            this.id = id;
        }

        public Discount(double percentage, string duration): base(percentage, duration)
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

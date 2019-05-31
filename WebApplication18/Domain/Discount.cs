using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public abstract class Discount : DiscountComponent

    {
       // protected double percentage;
        protected bool isPartOfComplex;
        //private int id;

        public Discount(double percentage, string duration): base(percentage, duration)
        {
            //this.percentage = percentage;
            this.isPartOfComplex = false;
        }

      /*  public double getPercentage()
        {
            return percentage;
        }
        public void setPercentage(double percentage)
        {
            this.percentage = percentage;
        }
        public string getDuration()
        {
            return duration;
        }*/


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

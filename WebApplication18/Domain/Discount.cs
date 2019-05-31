using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public abstract class Discount : DiscountComponent

    {
        protected double percentage;
        protected string duration;
        protected bool isPartOfComplex;
        //private int id;

        public Discount(double percentage, string duration): base()
        {
            this.percentage = percentage;
            this.duration = duration;
            this.isPartOfComplex = false;
        }

        public double getPercentage()
        {
            return percentage;
        }
        public string getDuration()
        {
            return duration;
        }
        public void setPercentage(double percentage)
        {
            this.percentage = percentage;
        }

        public bool getIsPartOfComplex() {
            return this.isPartOfComplex;
        }
        public void setIsPartOfComplex(bool isPartOfComplex)
        {
            this.isPartOfComplex = isPartOfComplex;
        }
    }
}

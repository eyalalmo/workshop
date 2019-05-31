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
        //private int id;

        public Discount(double percentage, string duration): base()
        {
            this.percentage = percentage;
            this.duration = duration;
        }

        internal Discount(int discountID, double percentage, string duration)
        {
            this.percentage = percentage;
            this.duration = duration;
            this.id = discountID;
        }

        public double getPercentage()
        {
            return percentage;
        }
        public string getDuration()
        {
            return duration;
        }
    }
}

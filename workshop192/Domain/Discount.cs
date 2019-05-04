using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public abstract class Discount : DiscountComponent

    {
        private double percentage;
        private string duration;

        public Discount(double percentage, string duration)
        {
            this.percentage = percentage;
            this.duration = duration;
        }
        abstract public bool checkCondition();
        public double getPercentage()
        {
            return percentage;
        }
        public string getDuration()
        {
            return duration;
        }
        public int getID()
        {
            return id;
        }

        
    }
}

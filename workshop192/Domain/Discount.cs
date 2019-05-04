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
        private int id;

        public Discount(double percentage, string duration, int id): base(id)
        {
            this.percentage = percentage;
            this.duration = duration;
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

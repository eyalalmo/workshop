using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class ReliantDiscount : Discount
    {
        private double percentage;
        private String condition;
        private String duration;
        public ReliantDiscount(double percentage, String condition, String duration)
        {
            this.percentage = percentage;
            this.condition = condition;
            this.duration = duration;
        }

        public bool meetsCondition(String condition)
        {

        }
    }
}

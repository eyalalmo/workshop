using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class VisibleDiscount : Discount
    {
        public VisibleDiscount(int percentage, string duration): base (percentage, duration) { }

        public override bool checkCondition()
        {
            throw new NotImplementedException();
        }
    }
}

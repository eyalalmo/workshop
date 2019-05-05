using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public abstract class DiscountComponent
    {
        int id;
        public DiscountComponent(int id)
        {
            this.id = id;
        }
        public int getId()
        {
            return this.id;
        }
     
        bool checkCondition();
    }
}

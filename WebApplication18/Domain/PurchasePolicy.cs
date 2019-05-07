using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public abstract class PurchasePolicy
    {
        public abstract void checkPolicy(Product p, int amount);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public abstract class PurchasePolicy
    {
        public abstract bool checkPolicy(int cartPrice, int amountofProd);
        public abstract void setAmount(int newAmount);
        public abstract int getAmount();

        public string description()
        {
            throw new NotImplementedException();
        }
    }
}

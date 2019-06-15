using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public abstract class PurchasePolicy
    {
        public abstract bool checkPolicy(double cartPrice, int amountofProd);
        public abstract void setAmount(int newAmount);
        public abstract int getAmount();

        public abstract string description();
        public abstract int getPolicyID();

        public abstract string getTypeString();
       
    }
}

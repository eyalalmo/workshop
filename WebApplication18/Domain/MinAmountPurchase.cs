using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using workshop192.Domain;

namespace workshop192.Domain
{
    public class MinAmountPurchase : PurchasePolicy
    {
        public int minAmount;

        public MinAmountPurchase(int minAmount)
        {
            this.minAmount = minAmount;
        }
        public override void checkPolicy(Product p, int amount)
        {
            if (amount < minAmount)
                throw new AlreadyExistException("can not purchase less than " + minAmount + " of the same product");
        }

        public override int getAmount()
        {
            return minAmount;
        }

        public override void setAmount(int newAmount)
        {
            if (newAmount <= 0)
            {
                throw new ArgumentOutOfRangeException("minimum amount can not be negative or 0");
            }
            minAmount = newAmount;
        }
    }
}
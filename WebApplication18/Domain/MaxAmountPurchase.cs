using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using workshop192.Domain;

namespace workshop192.Domain
{
    public class MaxAmountPurchase : PurchasePolicy
    {
        private int maxAmount;
        
        public MaxAmountPurchase(int maxAmount)
        {
            this.maxAmount = maxAmount;
        }
        public override void checkPolicy(Product p, int amount)
        {
            if (amount > maxAmount)
                throw new IllegalAmountException("can not purchase more than " + maxAmount + " of the same product");
        }
        public override void setAmount(int newAmount)
        {
            if (newAmount <= 0)
            {
                throw new ArgumentOutOfRangeException("maximum amount can not be negative or 0");
            }
            maxAmount = newAmount;
        }
        public  override int getAmount()
        {
            return maxAmount;
        }
    }
}
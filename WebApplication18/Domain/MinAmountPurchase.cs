using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using workshop192.Domain;

namespace WebApplication18.Domain
{
    public class MinAmountPurchase : PurchasePolicy
    {
        private int minAmount;

        public MinAmountPurchase(int minAmount)
        {
            this.minAmount = minAmount;
        }
        public override void checkPolicy(Product p, int amount)
        {
            if (amount < minAmount)
                throw new IllegalAmountException("can not purchase less than " + minAmount + " of the same product");
        }

    }
}
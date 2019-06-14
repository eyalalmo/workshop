using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using workshop192.Domain;

namespace workshop192.Domain
{
    public class MinAmountPurchase : PurchasePolicy
    {
        private int minAmount;
        private int policyID;

        public MinAmountPurchase(int minAmount)
        {
            this.minAmount = minAmount;
            this.policyID = DBStore.getInstance().getNextPolicyID();
        }
        public override bool checkPolicy(double cartPrice, int amountofProd)
        {
            if (minAmount > amountofProd)
                return false;
            return true;
        }

        public override string description()
        {
            return "Minimum amount of products is : " + minAmount + " ";
        }

        public override int getAmount()
        {
            return minAmount;
        }

        public override void setAmount(int newAmount)
        {
            if (newAmount <= 0)
            {
                throw new ArgumentOutOfRangeException("Error: Minimum amount can not be negative or 0");
            }
            minAmount = newAmount;
        }
    }
}
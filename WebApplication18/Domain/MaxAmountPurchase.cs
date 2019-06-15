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
        private int policyID;

        public MaxAmountPurchase(int maxAmount)
        {
            this.maxAmount = maxAmount;
            this.policyID = DBStore.getInstance().getNextPolicyID();
        }
        public override bool checkPolicy(double cartPrice, int amountofProd)
        {
            if (amountofProd > maxAmount)
                return false;
            return true;
        }
        public override void setAmount(int newAmount)
        {
            if (newAmount <= 0)
            {
                throw new ArgumentOutOfRangeException("Error: Maximum amount cannot be negative or 0");
            }
            maxAmount = newAmount;
        }
        public override int getAmount()
        {
            return maxAmount;
        }

        public override string description()
        {
            return "Maximum amount of product is: " + maxAmount + " ";
        }

        public override int getPolicyID()
        {
            return policyID;
        }

        public override string getTypeString()
        {
            return "Max Amount";
        }
    }
}
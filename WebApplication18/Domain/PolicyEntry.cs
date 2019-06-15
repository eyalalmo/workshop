using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication18.Domain
{
    public class PolicyEntry
    {
        private int storeID;
        private int policyID;
        private int amount;
        private string type;
        private string compType;
        private int subID1;
        private int subID2;
        private int isPartOfComplex;

       public PolicyEntry(int storeID, int policyID, string type, int amount, int subID1, int subID2,int isPartOfComplex)
        {
            this.storeID = storeID;
            this.type = type;
            this.amount = amount;
            this.policyID = policyID;
            this.subID1 = 1;
            this.subID2 = 2;
            this.isPartOfComplex = isPartOfComplex;
        }

        public int getStoreID() { return this.storeID; }
        public string getType() { return type; }
        public int getAmount() { return this.amount; }
        public int getPolicyID() { return this.policyID; }
        public bool getIsPartOfComp() { return isPartOfComplex==1; }
        public string getCompType() { return compType; }
        public int getSubID1() { return subID1; }
        public int getSubID2() { return subID2; }



    }
}
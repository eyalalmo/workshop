using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using workshop192.Domain;

namespace WebApplication18.Domain
{
    public class ComplexPurchasePolicy : PurchasePolicy
    {
        enum Type { or, xor, and };
        private PurchasePolicy p1;
        private PurchasePolicy p2;
//        private int storeID;
        private Type type;
        private int policyID;

        public ComplexPurchasePolicy(string type, PurchasePolicy p1, PurchasePolicy p2)
        {
            if (type == "xor")
                this.type = Type.xor;
            else if (type == "or")
                this.type = Type.or;
            else
                this.type = Type.and;
            this.p1 = p1;
            this.p2 = p2;
            this.policyID = DBStore.getInstance().getNextPolicyID();

        }
        public ComplexPurchasePolicy(string type, PurchasePolicy p1, PurchasePolicy p2, int policyID)
        {
            if (type == "xor")
                this.type = Type.xor;
            if (type == "or")
                this.type = Type.or;
            else
                this.type = Type.and;
            this.p1 = p1;
            this.p2 = p2;
            this.policyID = policyID;

        }
        public override string description()
        {
            return p1.description() + type.ToString()+" " + p2.description();
        }

        public override bool checkPolicy(double cartPrice, int amountofProd)
        {
            if (type == Type.and)
            {
                return checkAndType(cartPrice, amountofProd);
            }
            else if (type == Type.or)
            {
                return checkOrType(cartPrice, amountofProd);
            }
            else
            {
                return checkXorType(cartPrice, amountofProd);
            }
        }

        private bool checkXorType(double cartPrice, int amountofProd)
        {
            int count = 0;
            if (!p1.checkPolicy(cartPrice, amountofProd))
                count++;
            if (!p2.checkPolicy(cartPrice, amountofProd))
                count++;
            if (count != 1)
                return false;

            return true;
        }

        private bool checkOrType(double cartPrice, int amountofProd)
        {

            if (!p1.checkPolicy(cartPrice, amountofProd) && !p2.checkPolicy(cartPrice, amountofProd))
                return false;
            return true;

        }

        private bool checkAndType(double cartPrice, int amountofProd)
        {
            if (!p1.checkPolicy(cartPrice, amountofProd) || !p2.checkPolicy(cartPrice, amountofProd))
                return false;
            return true;
        }


        public override void setAmount(int newAmount)
        {
            throw new NotImplementedException();
        }

        public override int getAmount()
        {
            throw new NotImplementedException();
        }

        public override int getPolicyID() { return policyID;}

        public int getFirstChildID() { return p1.getPolicyID(); }

        public int getSecondChildID() { return p2.getPolicyID(); }
        public string getCompType()
        {
            if (type == Type.or)
                return "or";
            else if (type == Type.xor)
                return "xor";
            else
                return "and";
        }

        public PurchasePolicy getFirstPolicyChild() { return p1; }
        public PurchasePolicy getSecondPolicyChild() { return p2; }
        public override string getTypeString()
        {
            return "Complex Policy";
        }
    }

}
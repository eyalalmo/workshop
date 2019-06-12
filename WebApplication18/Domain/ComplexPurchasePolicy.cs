using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using workshop192.Domain;

namespace WebApplication18.Domain
{
    public class ComplexPurchasePolicy :PurchasePolicy
    {
        enum Type { OR, XOR, AND };
        private PurchasePolicy p1;
        private PurchasePolicy p2;
        private int storeID;
        private Type type;

        public ComplexPurchasePolicy(string type,PurchasePolicy p1, PurchasePolicy p2, int storeID) { 
            if (type == "XOR")
                this.type = Type.XOR;
            if (type == "OR")
                this.type = Type.OR;
            else
                this.type = Type.AND;
            this.p1 = p1;
            this.p2 = p2;
            
            this.storeID = storeID;

        }
        public string description()       {
            return p1.description() +type.ToString() + p2.description();
        }

        public bool checkPolicy(int cartPrice, int amountofProd)
        {
            if (type == Type.AND)
            {
                return checkTrueType(cartPrice, amountofProd);
            }
            else if (type == Type.OR)
            {
                return checkOrType(cartPrice, amountofProd);
            }
            else
            {
                return checkXorType(cartPrice, amountofProd);
            }
        }

        private bool checkXorType(int cartPrice, int amountofProd)
        {
            int count = 0;
            if (minAmount != -1)
            {
                if (minAmount < amountofProd)
                    count += 1;

            }

            if (maxAmount != -1)
            {
                if (maxAmount > amountofProd)
                    count += 1;
            }
            if (minTotalPrice != -1)
            {
                if (cartPrice < minTotalPrice)
                    count += 1;
            }

            if (complexChild != null)
            {
                if (complexChild.checkCondition(cartPrice, amountofProd))
                {
                    count += 1;
                }
            }
            if (count != 1)
                return false;

            return true;
        }

        private bool checkOrType(int cartPrice, int amountofProd)
        {
            int count = 0;
            if (minAmount != -1)
            {
                if (minAmount < amountofProd)
                    count += 1;

            }

            if (maxAmount != -1)
            {
                if (maxAmount > amountofProd)
                    count += 1;
            }
            if (minTotalPrice != -1)
            {
                if (cartPrice < minTotalPrice)
                    count += 1;
            }

            if(complexChild != null)
            {
                if (complexChild.checkCondition(cartPrice, amountofProd))
                {
                    count += 1;
                }
            }
            if (count == 0)
                return false;

            return true;

        }

        private bool checkTrueType(int cartPrice, int amountofProd)
        {
            if (minAmount != -1)
            {
                if (minAmount > amountofProd)
                    throw new ArgumentException("Error: Cannot purchase more than " + minAmount + " products in store: " + storeID);
            }

            if (maxAmount != -1)
            {
                if (maxAmount < amountofProd)
                    throw new ArgumentException("Error: Cannot purchase less than " + maxAmount + " products in store: " + storeID);
            }
            if (minTotalPrice != -1)
            {
                if (cartPrice < minTotalPrice)
                    throw new ArgumentException("Error: Cart total price must be above: " + minTotalPrice + " before checkout, in store: " + storeID + ".");
            }
            bool ans = true;
            if(complexChild != null)
            {
                ans = complexChild.checkPolicy(cartPrice, amountofProd);
            }
            return ans;
        }

        
        public override void setAmount(int newAmount)
        {
            throw new NotImplementedException();
        }

        public override int getAmount()
        {
            throw new NotImplementedException();
        }
    }

}
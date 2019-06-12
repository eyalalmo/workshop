using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using workshop192.Domain;

namespace WebApplication18.Domain
{
    public class ComplexPurchasePolicy
    {
        enum Type { OR, XOR, AND };

        private int minAmount;
        private int maxAmount;
        private int minTotalPrice;
        private ComplexPurchasePolicy complexChild;
        private Type type;

        public ComplexPurchasePolicy(string type, int minAmount, int maxAmount, int minTotalPrice, ComplexPurchasePolicy complexChild)
        {
            if (type == "XOR")
                this.type = Type.XOR;
            if (type == "OR")
                this.type = Type.OR;
            else
                this.type = Type.AND;

            this.minAmount = minAmount;
            this.maxAmount = maxAmount;
            this.minTotalPrice = minTotalPrice;
            this.complexChild = complexChild;

        }
        public string toString()
        {
            String ans = "";
            bool first = true;
            if (minAmount != -1)
            {
                ans += "Minimum products amount is: " + minAmount + " ";
                ans += type + " ";
                first = false;
            }
            if (maxAmount != -1)
            {
                ans += "Maximum products amount is: " + maxAmount + " ";
                if (first)
                    ans += type + " ";
                first = false;
            }

            if (minTotalPrice != -1)
            {
                ans += "Minimum price for the cart is: " + minTotalPrice + " ";
                if (first)
                    ans += type + " ";
                first = false;
            }
            if (complexChild != null)
            {
                ans += complexChild.toString();
            }

            return ans;

        }

        public bool checkCondition(int cartPrice, int amountofProd)
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
            throw new NotImplementedException();
        }

        private bool checkOrType(int cartPrice, int amountofProd)
        {
            throw new NotImplementedException();
        }

        private bool checkTrueType(int cartPrice, int amountofProd)
        {
            if (minAmount != -1)
            {
                if (minAmount < amountofProd)
                    throw new ArgumentException("Error: Cannot purchase more than " + maxPurchasePolicy + " products in store: " + storeId);
            }

            if (maxAmount != -1)
            {
                if (maxAmount > amountofProd)
                    throw new ArgumentException("Error: Cannot purchase less than " + maxPurchasePolicy + " products in store: " + storeId);
            }
            if(minTotalPrice != -1)
            {
                if (cartPrice < minTotalPrice)
                    throw new ArgumentException("Error: Cart total price must be above: " + minTotalPrice + " before checkout.");
            }
        }
    }

}
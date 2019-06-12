﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using workshop192.Domain;

namespace WebApplication18.Domain
{
    public class TotalPricePolicy : PurchasePolicy
    {
        private int totalPrice;
        public TotalPricePolicy(int totalPrice) 
        {

            if (totalPrice < 0)
            {
                throw new ArgumentException("total price can not be a negative number.");
            }

            this.totalPrice = totalPrice;
        }

        public override string description()
        {
            return "Minimum price for this cart is :" + totalPrice + " ";
        }

        public override bool checkPolicy(double cartPrice, int amountofProd)
        {
            if (cartPrice < totalPrice)
                return false;
            return true;
        }

        public override int getAmount()
        {
            return totalPrice;
        }

        public override void setAmount(int newAmount)
        {
            if (newAmount < 0)
            {
                throw new ArgumentException("total price can not be a negative number.");
            }
            this.totalPrice = newAmount;

        }
    }
}
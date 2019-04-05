﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class ShoppingBasket
    {
        private Dictionary<int,ShoppingCart> shoppingCarts;

        public ShoppingBasket()
        {
            this.shoppingCarts = new Dictionary<int, ShoppingCart>();
        }

        public Dictionary<int,ShoppingCart> getShoppingCarts()
        {
            return this.shoppingCarts;
        }


        public int totalAmount()
        {
            int sum = 0;
            foreach (ShoppingCart sc in shoppingCarts.Values)
            {
                sum += sc.totalAmount();
            }
            return sum;

        }
        public string addToCart(Product product, int amount)
        {
            int storeID = product.getStore().getStoreID();
            bool found = false;
            foreach(ShoppingCart sc in shoppingCarts.Values)
                if (sc.getStoreID() == storeID)
                {
                    string s = sc.addToCart(product,amount);
                    if (s.Equals(""))
                    {
                        found = true;
                        break;
                    }
                    else
                        return s;
                }
            if(!found)
            {
                ShoppingCart sc = new ShoppingCart(storeID);
                string s = sc.addToCart(product, amount);
                if (s.Equals(""))
                {
                    shoppingCarts.Add(storeID, sc);
                }
                else
                {
                    return s;
                }
            }
            return "";
        }
        public String checkout (String address,String creditCard){
            // return the result of the proccess by order of cart
            String output = "";
            foreach (ShoppingCart sc in shoppingCarts.Values)
            {
                output += sc.checkout(address,creditCard);
            }
            return output;
        }


        public ShoppingCart getShoppingCartByID(int storeID)
        {
            foreach (int id in shoppingCarts.Keys)
            {
                if (id == storeID)
                    return shoppingCarts[id];
            }
            return null;
        }



    }
}

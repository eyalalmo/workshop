﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class ShoppingCart
    {

        private Dictionary <Product, int> productList;
        private int storeID;

        public ShoppingCart(int storeID)
        {
            productList = new Dictionary<Product, int>();
            this.storeID = storeID;
        }
        public Dictionary<Product, int> getProductsInCarts()
        {
            return productList;
        }
        public int getStoreID()
        {
            return this.storeID;
        }

        public String addToCart(Product product, int amount)
        {
            int quantityLeft = product.getQuantityLeft();
            if ( quantityLeft - amount> 0)
            {
                //product.setQuantityLeft(quantityLeft - amount);
                productList.Add(product, amount);
                return "";
            }

            return "- product quantity";

        }

        public String removeFromCart(Product p)
        {
            if (!productList.ContainsKey(p))
               return "- product does not exist";
            productList.Remove(p);
            return "";
            
        }

        public String changeQuantityOfProduct(Product p, int newAmount)
        {
            if (!productList.ContainsKey(p))
                return "- product does not exist";
            int oldAmount = productList[p];
            int quantity = p.getQuantityLeft();
            if (quantity + oldAmount - newAmount < 0)
                return "- quantity below zero";
            //p.setQuantityLeft(quantity + oldAmount - newAmount);
            productList.Remove(p);
            productList.Add(p, newAmount);
            return "";

        }


        public String checkout() {
            String res = "";
            int sum = 0;
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                if (entry.Key.getQuantityLeft() > entry.Value)
                {
                    
                    sum = entry.Key.getPrice() * entry.Value;
                    Boolean isOk = PaymentService.getInstance().checkOut("496531",sum);
                    if (isOk)
                    {
                        entry.Key.setQuantityLeft(entry.Key.getQuantityLeft() - entry.Value);

                        if (DeliveryService.getInstance().sendToUser("beer sheva", entry.Key) == false)
                        {
                            entry.Key.setQuantityLeft(entry.Key.getQuantityLeft() + entry.Value);
                            res += " product: " + entry.Key.getProductID() + " can't deliver.\n ";
                        }
                        else
                        {
                            res += " product: " + entry.Key.getProductID() + " complete payment.\n ";
                        }

                        //////////add eilon part
                    }
                    else
                    {
                        res += " product: " + entry.Key.getProductID() + " cant submmit checkout.\n ";
                    }
                }
                else
                {
                    res += " product: " + entry.Key.getProductID() + " have no this Quantity.\n ";
                }

            }
            return res;


        } 


    }
}

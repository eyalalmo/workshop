﻿using System;
using workshop192.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Bridge;

namespace workshop192.ServiceLayer
{
    public class BasketService
    {
        private static BasketService instance;
        private DomainBridge db = DomainBridge.getInstance();

        public static BasketService getInstance() {
            if (instance == null)
                instance = new BasketService();
            return instance;
        }

        private BasketService()
        {

        }
        //use case 2.7
        public Dictionary<int, ShoppingCart> getShoppingCarts(Session user)
        {
            return db.getShoppingCarts(user);
        }

        public ShoppingCart getCart(Session user, int store)
        {
            if (store < 0)
            {
                throw new ArgumentException("invalid store id");
            }

            return db.getCart(user,store);
        }
        //use case 2.6
        public void addToCart(Session user,int product,int amount)
        {
            if (product < 0)
            {
                throw new ArgumentException("invalid product id");
            }

            if (amount <= 0)
            {
                throw new IllegalAmountException("error : amount should be a positive number");
            }
            db.addToCart(user, product, amount);
            
        }
        //use case 2.7
        public void removeFromCart(Session user,int store, int product)
        {
            if (product < 0)
            {
                throw new ArgumentException("invalid product id");
            }

            if (store < 0)
            {
                throw new ArgumentException("invalid store id");
            }
            db.removeFromCart(user, store, product);
        }
        //use case 2.7
        public void changeQuantity(Session user, int product,int store, int newAmount)
        {
            if (product < 0)
            {
                throw new ArgumentException("invalid product id");
            }

            if (store < 0)
            {
                throw new ArgumentException("invalid store id");
            }

            if (newAmount <= 0)
            {
                throw new IllegalAmountException( "ERROR: quantity should be a positive number");
            }

            db.changeQuantity(user, product, store, newAmount);
        }

        public void checkoutCart(Session user,int store,String address,String creditCard){
            if (store < 0)
            {
                throw new ArgumentException("invalid store id");
            }

             db.checkoutCart(user, store, address, creditCard);
        }

        public void checkoutBasket(Session user, String address, String creditCard)
        {
            db.checkoutBasket(user, address, creditCard);
        }                                               
    }
}

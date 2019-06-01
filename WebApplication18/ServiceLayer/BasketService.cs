using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Bridge;
using workshop192.Domain;

namespace workshop192.ServiceLayer
{
    public class BasketService
    {
        private static BasketService instance;
        private DomainBridge db = DomainBridge.getInstance();

        public static BasketService getInstance()
        {
            if (instance == null)
                instance = new BasketService();
            return instance;
        }

        private BasketService()
        {

        }
       
      

       
        //use case 2.6
        public void addToCart(int user, int product, int amount)
        {
            if (product < 0)
            {
                throw new ArgumentException("invalid product id");
            }

            if (amount <= 0)
            {
                throw new AlreadyExistException("error : amount should be a positive number");
            }
            db.addToCart(user, product, amount);

        }
        //use case 2.7
        public void removeFromCart(int user, int product)
        {
            if (product < 0)
            {
                throw new ArgumentException("invalid product id");
            }
            db.removeFromCart(user, product);
        }
        //use case 2.7
        public void changeQuantity(int user, int product, int newAmount)
        {
            if (product < 0)
            {
                throw new ArgumentException("invalid product id");
            }

            if (newAmount <= 0)
            {
                throw new AlreadyExistException("ERROR: quantity should be a positive number");
            }

            db.changeQuantity(user, product, newAmount);
        }

        /*public void checkoutCart(int user,int store,String address,String creditCard){
            if (store < 0)
            {
                throw new ArgumentException("invalid store id");
            }

             db.checkoutCart(user, store, address, creditCard);
        }*/

        public void checkoutBasket(int user, String address, String creditCard)
        {
            db.checkoutBasket(user, address, creditCard);
        }

        public double getActualTotalPrice(int user)
        {
            return db.getShoppingBasketActualTotalPrice(user);
        }
        public double getTotalPrice(int user)
        {
            return db.getShoppingBasketTotalPrice(user);
        }

        public double getProductPrice(int productid)
        {
            if (productid < 0)
                throw new ArgumentException("invalid product id");
            return db.getProductPrice(productid);
        }

        //costumer adds coupon to cart
        public void addCouponToCart(int sessionID, int storeID, string couponCode)
        {
            db.addcouponToCart(sessionID, storeID, couponCode);
        }
        // costumer removes coupon from cart
        public void removeCouponFromCart(int sessionID, int storeID)
        {
            db.removeCouponFromCart(sessionID, storeID);
        }

        public double getAmountOfCart(int storeID, int sessionID)
        {
            return db.getAmountByCart(storeID, sessionID);
        }
    }
}
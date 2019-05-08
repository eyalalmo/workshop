using System;
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
        public Dictionary<int, ShoppingCart> getShoppingCarts(int user)
        {
            return db.getShoppingCarts(user);
        }

        public ShoppingCart getCart(int user, int store)
        {
            if (store < 0)
            {
                throw new ArgumentException("invalid store id");
            }

            return db.getCart(user,store);
        }
        //use case 2.6
        public void addToCart(int user,int product,int amount)
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
        public void removeFromCart(int user,int product)
        {
            if (product < 0)
            {
                throw new ArgumentException("invalid product id");
            }
            db.removeFromCart(user, product);
        }
        //use case 2.7
        public void changeQuantity(int user, int product,int store, int newAmount)
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
                throw new AlreadyExistException( "ERROR: quantity should be a positive number");
            }

            db.changeQuantity(user, product, store, newAmount);
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
    }
}

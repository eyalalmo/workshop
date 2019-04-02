using System;
using workshop192.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.ServiceLayer
{
    class BasketService
    {
        private static BasketService instance;

        public static BasketService getInstance() {
            if (instance == null)
                instance = new BasketService();
            return instance;
        }

        private BasketService()
        {

        }

        public Dictionary<int, ShoppingCart> getShoppingCarts(Session user)
        {
            return user.getShoppingBasket().getShoppingCarts();
     
        }

        public ShoppingCart getCart(Session user, String storeID)
        {
            return user.getShoppingBasket().getShoppingCart(storeID);
        }

        public String addToCart(ShoppingCart cart,Product product,int amount)
        {
            return cart.addToCart(product, amount);
        }

        public String removeFromCart(ShoppingCart cart, Product product)
        {
            return cart.removeFromCart(product);
        }

        public String changeQuantity(ShoppingCart cart, Product product, int newAmount)
        {
            return cart.changeQuantityOfProduct(product,newAmount);
        }

    }
}

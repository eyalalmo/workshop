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
        //use case 2.7
        public Dictionary<int, ShoppingCart> getShoppingCarts(Session user)
        {
            return user.getShoppingBasket().getShoppingCarts();
     
        }

        public ShoppingCart getCart(Session user, Store store)
        {
            return user.getShoppingBasket().getShoppingCartByID(store.getStoreID());
        }
        //use case 2.6
        public String addToCart(Session user, Store store,Product product,int amount)
        {
            return user.getShoppingBasket().getShoppingCartByID(store.getStoreID()).addToCart(product, amount);
        }
        //use case 2.7
        public String removeFromCart(Session user,Store store, Product product)
        {
             return user.getShoppingBasket().getShoppingCartByID(store.getStoreID()).removeFromCart(product);
        }
        //use case 2.7
        public String changeQuantity(Session user, Product product,Store store, int newAmount)
        {
            return user.getShoppingBasket().getShoppingCartByID(store.getStoreID()).changeQuantityOfProduct(product,newAmount);
        }

        public String checkoutCart(Session user,Store store){
            return user.getShoppingBasket().getShoppingCartByID(store.getStoreID()).checkout();
        }

        public String checkoutBasket(Session user){
            return user.getShoppingBasket().checkout();
        }                                               
    }
}

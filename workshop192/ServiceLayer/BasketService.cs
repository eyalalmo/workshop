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
        private static DomainBridge domainBridge;

        public static BasketService getInstance() {
            if (instance == null)
            {
                instance = new BasketService();
                domainBridge = DomainBridge.getInstance();
            }
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
        public void addToCart(Session user, Store store,Product product,int amount)
        {
            if (amount <= 0)
            {
                throw new IllegalAmountException("error : amount should be a positive number");
            }
            user.getShoppingBasket().addToCart(product, amount);
            
        }
        //use case 2.7
        public void removeFromCart(Session user,Store store, Product product)
        {
              user.getShoppingBasket().getShoppingCartByID(store.getStoreID()).removeFromCart(product);
        }
        //use case 2.7
        public void changeQuantity(Session user, Product product,Store store, int newAmount)
        {
            if (newAmount <= 0)
            {
                throw new IllegalAmountException( "ERROR: quantity should be a positive number");
            }

            user.getShoppingBasket().getShoppingCartByID(store.getStoreID()).changeQuantityOfProduct(product,newAmount);
        }

        public string checkoutCart(Session user,Store store,String address,String creditCard){
            return user.getShoppingBasket().getShoppingCartByID(store.getStoreID()).checkout(address,creditCard);
        }

        public String checkoutBasket(Session user, String address, String creditCard)
        {
            return user.getShoppingBasket().checkout(address, creditCard);
        }                                               
    }
}

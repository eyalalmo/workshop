using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;
using workshop192.ServiceLayer;

namespace UnitTestProject3
{
    [TestClass]
    public class shopingbasketTest
    {

        public Store store;
        public ShoppingCart cart;
        ShoppingBasket basket; 

        [TestInitialize]
        public void initial()
        {
            DBProduct.getInstance().init();
            DBProduct.getInstance().init();
            store = new Store("store1", "games store");
            cart = new ShoppingCart(store.getStoreID());
            basket = new ShoppingBasket();
        }


        [TestMethod]
        public void addToCartTest1()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store);
            basket.addToCart(p1, 5);
            Assert.AreEqual(basket.getShoppingCarts().Keys.Count, 1);
        }
    }
}

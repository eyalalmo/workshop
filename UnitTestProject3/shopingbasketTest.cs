using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;
using workshop192.ServiceLayer;

namespace UnitTestProject3
{
    [TestClass]
    public class shopingbasketTest
    {
        private UserService userService = UserService.getInstance();
        public StoreService storeService = StoreService.getInstance();
        public ShoppingCart cart;
        public ShoppingBasket basket;
        public int session1;
        public int store1;

        [TestInitialize]
        public void initial()
        {
            userService.setup();
            DBProduct.getInstance().init();
            DBStore.getInstance().init();
            session1 = userService.startSession();// login 
            userService.register(session1, "user1", "user1");
            userService.login(session1, "user1", "user1");

            store1 = storeService.addStore("billabong", "clothing", session1);

            cart = new ShoppingCart(store1);
            basket = new ShoppingBasket();


        }
        [TestMethod]
        public void addToCartTest1()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, DBStore.getInstance().getStore(store1));
            basket.addToCart(p1, 5);
            Assert.AreEqual(basket.getShoppingCarts().Keys.Count, 1);
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;
using workshop192.ServiceLayer;

namespace UnitTestProject3
{
    [TestClass]
    public class shopingCartTests
    {
        public Store store;
        public ShoppingCart cart;
        public int session;
        public UserService userService = UserService.getInstance();

        [TestInitialize]
        public void initial()
        {
            userService.setup();
            DBProduct.getInstance().init();
            DBProduct.getInstance().init();
            DBStore.getInstance().init();
            store = new Store("store1", "games store");
            DBStore.getInstance().addStore(store);

            cart = new ShoppingCart(store.getStoreID());

        }

        [TestMethod]
        public void TestAddProduct1()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store.getStoreID());
            Product p2 = new Product("p2", "ff", 56, 2, 10, store.getStoreID());
            Product p3 = new Product("p3", "ff", 56, 2, 10, store.getStoreID());
            cart.addToCart(p1, 5);
            cart.addToCart(p2, 5);
            cart.addToCart(p3, 5);

            Assert.AreEqual(3, cart.getProductsInCarts().Keys.Count);



        }
        [TestMethod]
        public void TestAddProduct2()
        {

            Product p1 = new Product("p1", "ff", 56, 2, 10, store.getStoreID());
            Product p2 = new Product("p2", "ff", 56, 2, 10, store.getStoreID());
            cart.addToCart(p1, 5);
            cart.addToCart(p2, 5);

            Assert.AreNotEqual(3, cart.getProductsInCarts().Keys.Count);

        }
        [TestMethod]
        public void TestAddProduct3()
        {
            Assert.AreEqual(0, cart.getProductsInCarts().Keys.Count);

        }
        [TestMethod]
        public void TestRemoveProduct1()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store.getStoreID());
            Product p2 = new Product("p2", "ff", 56, 2, 10, store.getStoreID());
            cart.addToCart(p1, 5);
            cart.addToCart(p2, 5);
            cart.removeFromCart(p1);
            Assert.AreEqual(1, cart.getProductsInCarts().Keys.Count);
        }
        [TestMethod]
        public void TestRemoveProduct2()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store.getStoreID());
            cart.addToCart(p1, 5);
            cart.removeFromCart(p1);
            Assert.AreEqual(0, cart.getProductsInCarts().Keys.Count);
        }

        [TestMethod]
        public void changeQuantityOfProductTest1()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store.getStoreID());
            cart.addToCart(p1, 5);
            cart.changeQuantityOfProduct(p1, 7);
            Assert.AreEqual(7, cart.getProductsInCarts()[p1]);
        }
        [TestMethod]
        public void changeQuantityOfProductTest2()
        {
            try
            {
                Product p1 = new Product("p1", "ff", 56, 2, 10, store.getStoreID());
                cart.addToCart(p1, 5);
                cart.changeQuantityOfProduct(p1, 20);
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void totalAmountTest1()
        {
            Product p1 = new Product("p1", "ff", 10, 2, 10, store.getStoreID());
            Product p2 = new Product("p2", "ff", 5, 2, 10, store.getStoreID());
            Product p3 = new Product("p3", "ff", 5, 2, 10, store.getStoreID());
            cart.addToCart(p1, 2);
            cart.addToCart(p2, 1);
            cart.addToCart(p3, 3);
            double fdsfds = cart.getTotalPrice();
            Assert.AreEqual(fdsfds, 40);
        }
    }
      
}



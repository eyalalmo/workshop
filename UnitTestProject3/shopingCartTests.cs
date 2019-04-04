using System;
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

       [TestInitialize]
        public void initial()
        {

            store = new Store("store1", "games store");
             cart = new ShoppingCart(store.getStoreID());     
        }


        [TestMethod]
        public void TestAddProduct1()
        {
                           
                Product p1 = new Product("p1", "ff", 56, 2, 10, store);
                Product p2 = new Product("p2", "ff", 56, 2, 10, store);
                Product p3 = new Product("p3", "ff", 56, 2, 10, store);
                cart.addToCart(p1, 5);
                cart.addToCart(p2, 5);
                cart.addToCart(p3, 5);
                
                Assert.AreEqual(3, cart.getProductsInCarts().Keys.Count);
           
               
            
        }
        [TestMethod]
        public void TestAddProduct2()
        {
           
            Product p1 = new Product("p1", "ff", 56, 2, 10, store);
            Product p2 = new Product("p2", "ff", 56, 2, 10, store);
            cart.addToCart(p1, 5);
            cart.addToCart(p2, 5);
          
            Assert.AreNotEqual(3, cart.getProductsInCarts().Keys.Count);

        }
        [TestMethod]
        public void TestAddProduct3()
        {
            Assert.AreEqual(0, cart.getProductsInCarts().Keys.Count);

        }
        public void TestRemoveProduct1()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store);
            Product p2 = new Product("p2", "ff", 56, 2, 10, store);
            cart.addToCart(p1, 5);
            cart.addToCart(p2, 5);
            Assert.AreNotEqual(3, cart.getProductsInCarts().Keys.Count);

        }
    }
}

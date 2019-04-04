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
        public Store store1;
        public ShoppingCart cart;
        ShoppingBasket basket; 

        [TestInitialize]
        public void initial()
        {
            DBProduct.getInstance().init();
            DBProduct.getInstance().init();
            store = new Store("store1", "games store");
            store1 = new Store("store2", "games store");
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

        [TestMethod]
        public void addToCartTest2()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store);
            Product p2 = new Product("p2", "ff", 56, 2, 10, store);
            basket.addToCart(p1, 5);
            basket.addToCart(p2, 5);
            Assert.AreEqual(basket.getShoppingCarts().Keys.Count, 1);
        }
        [TestMethod]
        public void addToCartTest3()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store);
            Product p2 = new Product("p2", "ff", 56, 2, 10, store1);
            basket.addToCart(p1, 5);
            basket.addToCart(p2, 5);  
            Assert.AreEqual(basket.getShoppingCarts().Keys.Count,2);
        }

        [TestMethod]
        public void addToCartTest4()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store);
            Product p2 = new Product("p2", "ff", 56, 2, 10, store1);
            basket.addToCart(p1, 5);
            basket.addToCart(p2, 5);
            basket.addToCart(p2, 3);
            Assert.AreEqual(basket.addToCart(p2, 3), " product already exist");
        }
        [TestMethod]
        public void checkout1()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store);
            Product p2 = new Product("p2", "ff", 56, 2, 10, store1);
            Product p3 = new Product("p3", "ff", 56, 2, 10, store1);
            basket.addToCart(p1, 5);
            basket.addToCart(p2, 5);
            basket.addToCart(p3, 3);
            Assert.AreEqual(basket.checkout("beer sheva","2222223"), " product: 0 complete payment.  product: 1 complete payment.  product: 2 complete payment. ");
        }

        [TestMethod]
        public void checkout2()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store);
            basket.addToCart(p1, 5);
            Assert.AreEqual(basket.checkout("beer sheva", "2222223"), " product: 0 complete payment. ");
        }
       

        [TestMethod]
        public void getShoppingCartByID1()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store);
            Product p3 = new Product("p3", "ff", 56, 2, 10, store);
            basket.addToCart(p1, 5);
            basket.addToCart(p3, 3);
            Assert.AreEqual(basket.getShoppingCartByID(store.getStoreID()), basket.getShoppingCarts()[store.getStoreID()]);
        }
    }
}

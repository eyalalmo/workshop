using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.ServiceLayer.Tests
{
    [TestClass()]
    public class BasketServiceTests
    {

        //2.6
        [TestMethod()]
        public void addToCartTest()
        {
            UserService userService = UserService.getInstance();
            StoreService storeService = StoreService.getInstance();
            Session session = userService.startSession();
            Store store1 = storeService.addStore("zebra", "Clothes", session);
            Store store2 = storeService.addStore("iFix", "Technology", session);
           
            storeService.addProduct("dress", "clothing", 20, 5, 3, store1, session);
            storeService.addProduct("coat", "clothing", 100, 2, 4, store1, session);
            storeService.addProduct("hat", "clothing", 200, 3, 2, store1, session);
            storeService.addProduct("iPhone XS", "Technology", 120, 1, 2, store2, session);
            storeService.addProduct("galaxy X", "Technology", 110, 4, 0, store2, session);

            LinkedList<Product> products1 = store1.getProductList();
            Product p1 = products1.ElementAt(0);
            Product p2 = products1.ElementAt(1);
            Product p3 = products1.ElementAt(2);

            LinkedList<Product> products2 = store2.getProductList();
            Product p4 = products2.ElementAt(0);
            Product p5 = products2.ElementAt(1);


            BasketService basketService = BasketService.getInstance();

            p1.setQuantityLeft(2);
            p2.setQuantityLeft(0);
            p3.setQuantityLeft(10);

            p4.setQuantityLeft(2);
            p5.setQuantityLeft(1);
            string s1= basketService.addToCart(session, store1, p1, 1); //ok
            string s2 = basketService.addToCart(session, store1, p2, 2); // should not succesed 
            string s3 = basketService.addToCart(session, store1, p3, 5); //ok

            string s4 = basketService.addToCart(session, store2, p4, 3); // should not succesed 
            string s5 = basketService.addToCart(session, store2, p5, 5); //ok

            Assert.Equals("", s1);
            Assert.AreNotEqual("", s2);
            Assert.Equals("", s3);

            Assert.AreNotEqual("", s4);
            Assert.Equals("", s5);
            
          /*  ShoppingCart shoppingCart1 = session.getShoppingBasket().getShoppingCartByID(store1.getStoreID());
            ShoppingCart shoppingCart2 = session.getShoppingBasket().getShoppingCartByID(store2.getStoreID());

            Dictionary<Product, int> productsInSC1 = shoppingCart1.getProductsInCarts();
            Dictionary<Product, int> productsInSC2 = shoppingCart2.getProductsInCarts();

            Assert.AreEqual(true, productsInSC1.ContainsKey(p1));
            Assert.AreEqual(false, productsInSC1.ContainsKey(p2));
            Assert.AreEqual(true, productsInSC1.ContainsKey(p3));

            Assert.AreEqual(false, productsInSC2.ContainsKey(p4));
            Assert.AreEqual(true, productsInSC2.ContainsKey(p5));
            */



        }

        [TestMethod()]
        public void watchShoppingBasket()
        {
            UserService userService = UserService.getInstance();
            StoreService storeService = StoreService.getInstance();
            Session session = userService.startSession();
            Store store1 = storeService.addStore("zara", "Clothes", session);
            Store store2 = storeService.addStore("bikeMe", "BikeStore", session);

            storeService.addProduct("dress", "clothing", 20, 5, 3, store1, session);
            storeService.addProduct("coat", "clothing", 100, 2, 4, store1, session);
            storeService.addProduct("hat", "clothing", 200, 3, 2, store1, session);
            storeService.addProduct("scooter", "vehicle", 120, 1, 2, store2, session);
            storeService.addProduct("Bicycle", "vehicle", 110, 4, 0, store2, session);

            BasketService basketService = BasketService.getInstance();

            ShoppingCart shoppingCart1 = basketService.getCart(session, store1);



        }

    }
    }
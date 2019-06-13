using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace workshop192.ServiceLayer.Tests
{
    [TestClass()]
    public class BasketServiceTests
    {
        UserService userService = UserService.getInstance();
        StoreService storeService = StoreService.getInstance();
        BasketService basketService = BasketService.getInstance();

        
        int session1;
        int session2;
        int session3;

        int store1;
        int store3;
        int store4;
        int store5;

        int p1;
        int p2;
        int p3;

        int p4;
        int p5;

        int p6;
        int p7;
        int p8;

        [TestInitialize()]
        public void initial()
        {

            userService.testSetup();

            session1 = userService.startSession();
            userService.register(session1, "user1", "user1");
            userService.login(session1, "user1", "user1");

            store1 = storeService.addStore("zebra", "Clothes", session1);
            p1 = storeService.addProduct("dress", "clothing", 20, 5, 2, store1, session1);
            p2 = storeService.addProduct("coat", "clothing", 100, 2, 3, store1, session1);
            p3 = storeService.addProduct("hat", "clothing", 200, 3, 4, store1, session1);

            session2 = userService.startSession();
            userService.register(session2, "user2", "user2");
            userService.login(session2, "user2", "user2");
            store3 = storeService.addStore("zara", "Clothes", session2);
            store4 = storeService.addStore("bikeMe", "BikeStore", session2);

            p4 = storeService.addProduct("coat", "clothing", 100, 2, 2, store3, session2);
            p5 = storeService.addProduct("hat", "clothing", 200, 3, 3, store3, session2);

            session3 = userService.startSession();
            userService.register(session3, "user3", "user3");
            userService.login(session3, "user3", "user3");

            store4 = storeService.addStore("TopTen", "Accesorize", session3);
            store5 = storeService.addStore("H&M", "clothing", session3);


            p6 = storeService.addProduct("Earings", "Accesorize", 76, 5, 7, store5, session3);
            p7 = storeService.addProduct("necklace", "Accesorize", 100, 2, 4, store5, session3);
            p8 = storeService.addProduct("ring", "Accesorize", 200, 3, 39, store5, session3);

        }

        //2.6 /
        [TestMethod()]
        public void addToCartTestSuccess()
        {
            try
            {
                basketService.addToCart(session1, p1, 1);
                basketService.addToCart(session1, p2, 2);

            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }

        }

        [TestMethod()]
        public void addToCartTestFail()
        {
            try
            {
                basketService.addToCart(session1, p3, 6); // should fail - too much 
                Assert.Fail();

            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }


        }

        [TestMethod()]
        public void watchShoppingBasketSuccess()
        {
            try
            {
                basketService.addToCart(session2, p4, 2);
                basketService.changeQuantity(session2, p4, 1);
                Assert.Fail();

            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }

        }

        [TestMethod()]
        public void watchShoppingBasketFail()
        {
            try
            {
                basketService.addToCart(session2, p5, 2);
                basketService.changeQuantity(session2, p5, 9);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);

            }
        }
        
        [TestMethod()]
        public void removeFromShoppingCartsuccess()
        {
            try
            {
                basketService.addToCart(session3, p6, 1);
                basketService.addToCart(session3, p7, 3);
                basketService.addToCart(session3, p8, 10);

                double amount = basketService.getAmountOfCart(store5, session3);
                basketService.removeFromCart(session3, p6);

                Assert.AreNotEqual(amount, basketService.getAmountOfCart(store5,session3));

                double newAmount = amount - basketService.getProductPrice(p6);

                Assert.AreEqual(newAmount, basketService.getAmountOfCart(store5, session3));
            }
            catch (Exception) {
                Assert.Fail();
            }
        }
    }
}
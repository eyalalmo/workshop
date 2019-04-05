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
        BasketService basketService;
        UserService userService;
        StoreService storeService;
        Session session2;
        Store store1;
        Store store2;
       
        Product p1;
        Product p2;
        Product p3;
        Product p4;
        Product p5;

        [TestInitialize()]
        public void initial()
        {
            basketService = BasketService.getInstance();

            userService = UserService.getInstance();
            storeService = StoreService.getInstance();
            
            
        }

        //2.6
        [TestMethod()]
        public void addToCartTest()
        {
            
            UserService user = UserService.getInstance();
            Session session1 = userService.startSession();
            user.register(session1, "user", "user");
            user.login(session1, "user", "user");

            store1 = storeService.addStore("zebra", "Clothes", session1);
            store2 = storeService.addStore("iFix", "Technology", session1);
            p1 = storeService.addProduct("dress", "clothing", 20, 5, 2, store1,session1);
            p2 = storeService.addProduct("coat", "clothing", 100, 2, 1, store1, session1);
            p3 = storeService.addProduct("hat", "clothing", 200, 3, 10, store1, session1);
            p4 = storeService.addProduct("iPhone XS", "Technology", 120, 1, 2, store2, session1);
            p5 = storeService.addProduct("galaxy X", "Technology", 110, 4, 7, store2, session1);

            string s1 = basketService.addToCart(session1, store1, p1, 1); //ok
            string s2 = basketService.addToCart(session1, store1, p2, 2); // should not succesed 
            string s3 = basketService.addToCart(session1, store1, p3, 5); //ok

            string s4 = basketService.addToCart(session1, store2, p4, 3); // should not succesed 
            string s5 = basketService.addToCart(session1, store2, p5, 5); //ok

            Assert.AreEqual("", s1);
            Assert.AreNotEqual("", s2);
            Assert.AreEqual("", s3);

            Assert.AreNotEqual("", s4);
            Assert.AreEqual("", s5);
            


        }

        [TestMethod()]
        public void watchShoppingBasket()
        {
            UserService user = UserService.getInstance();
            session2 = userService.startSession();
            user.register(session2, "user2", "user2");
            user.login(session2, "user2", "user2");


            Store store3 = storeService.addStore("zara", "Clothes", session2);
            Store store4 = storeService.addStore("bikeMe", "BikeStore", session2);
            
            
            Product p6 = storeService.addProduct("dress", "clothing", 20, 5, 1, store3, session2);
            Product p7 = storeService.addProduct("coat", "clothing", 100, 2, 2, store3, session2);
            Product p8 = storeService.addProduct("hat", "clothing", 200, 3, 3, store3, session2);
            Product p9 = storeService.addProduct("scooter", "vehicle", 120, 1, 7, store4, session2);
            Product p10 = storeService.addProduct("Bicycle", "vehicle", 110, 4, 9, store4, session2);

            string s1 = basketService.addToCart(session2, store3, p6, 1);
            string s2 = basketService.addToCart(session2, store3, p7, 2);
            string s3 = basketService.addToCart(session2, store3, p8, 2);
            string s4 = basketService.addToCart(session2, store4, p9, 3);
            string s5 = basketService.addToCart(session2, store4, p10, 5);

            string s7 = basketService.removeFromCart(session2, store3, p6);
            Assert.AreEqual("", s7);

            string s8 = basketService.changeQuantity(session2, p7, store3, 1);
            Assert.AreEqual("", s7);

            string s9 = basketService.changeQuantity(session2, p8, store3, 9);
            Assert.AreNotEqual("", s9);



            string s10 = basketService.changeQuantity(session2, p9, store4, 3);
            Assert.AreEqual("", s10);

            string s11 = basketService.changeQuantity(session2, p10, store4, 20);
            Assert.AreNotEqual("", s11);

            string s12 = basketService.removeFromCart(session2, store4, p9);
            Assert.AreEqual("", s12);




        }

    }
}
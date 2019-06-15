


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
    public class UserServiceTests
    {
        private UserService userService = UserService.getInstance();
        private StoreService storeService = StoreService.getInstance();
        private BasketService basketService = BasketService.getInstance();
        private int session1, session2; // session3;


        [TestInitialize()]
        public void Initialize()
        {
            userService.testSetup();

            session1 = userService.startSession();
            session2 = userService.startSession();

            userService.register(session1, "zubu", "mafu");
            userService.login(session1, "zubu", "mafu");

            int store1 = userService.createStore(session1, "Golf", "Clothes");
            int store2 = userService.createStore(session1, "Shiomi", "Technology");
            storeService.addProduct("shirt", "clothes", 20, 5, 2, store1, session1);
            storeService.addProduct("pan", "kitchen", 100, 2, 4, store1, session1);
            storeService.addProduct("stove", "kitchen", 200, 3, 2, store1, session1);
            storeService.addProduct("pants", "clothes", 120, 1, 2, store2, session1);
            storeService.addProduct("socks", "clothes", 110, 4, 2, store2, session1);

        }
        //1
        [TestMethod]
        public void initialTest()
        {
         
            try
            {
              bool pay=  userService.handShakePay();
              bool deliver = userService.handShakeDeliver();
                if(pay==false || deliver == false)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(true, true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);


        }

        //2.2
        [TestMethod]
        public void registerSuccessTest()
        {
            try
            {
                userService.register(session2, "user", "user");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
            
        }

        public void registerFailTest()
        {
            try
            {
                registerSuccessTest();
                userService.register(session2, "user", "user");
                Assert.Fail();
            }
            catch (RegisterException)
            {
                Assert.IsTrue(true);
            }
        }
        //2.3
        public void loginSuccessTest()
        {
            try
            {
                registerSuccessTest();
                userService.login(session2, "user", "user");
            }
            catch (RegisterException)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }

        public void loginFailTest()
        {
            try
            {
                registerSuccessTest();
                userService.login(session2, "user", "user33");
                Assert.Fail();
            }
            catch (RegisterException)
            {
                Assert.IsTrue(true);

            }

        }

        public void loginFail2Test()
        {
            try
            {
                userService.login(session2, "user", "user");
                Assert.Fail();
            }
            catch (RegisterException)
            {
                Assert.IsTrue(true);

            }

        }
        //2.5.1+2.5.2

        [TestMethod]
        public void searchByCategoryTest1()
        {
            loginSuccessTest();
            string searchResult1 = userService.searchByCategory("kitchen");
            int storeid = (DBStore.nextStoreID - 1);
            Assert.AreEqual(searchResult1, "[{\"productID\":2,\"productName" +
                "\":\"stove\",\"productCategory\":\"kitchen\",\"price\":200,\"rank" +
                "\":3,\"quantityLeft\":2,\"storeID\":" + storeid + "},{\"productID\":1," +
                "\"productName\":\"pan\",\"productCategory\":\"kitchen\"," +
                "\"price\":100,\"rank\":2,\"quantityLeft\":4,\"storeID" +
                "\":" + storeid + "}]");
        }

        [TestMethod]
        public void searchByCategoryTest2()
        {
            loginSuccessTest();
            string searchResult2 = userService.searchByCategory("clothes");
            int storeid = (DBStore.nextStoreID);
            Assert.AreEqual(searchResult2, "[{\"productID\":4,\"productName" +
                "\":\"socks\",\"productCategory\":\"clothes\",\"price\":110," +
                "\"rank\":4,\"quantityLeft\":2,\"storeID\":" + storeid + "},{\"productID" +
                "\":3,\"productName\":\"pants\",\"productCategory\":\"clothes" +
                "\",\"price\":120,\"rank\":1,\"quantityLeft\":2,\"storeID" +
                "\":" + storeid + "},{\"productID\":0,\"productName\":\"shirt\"," +
                "\"productCategory\":\"clothes\",\"price\":20,\"rank\":5," +
                "\"quantityLeft\":2,\"storeID\":" + (storeid - 1) + "}]");
        }
        [TestMethod]
        public void searchByCategoryFail()
        {
            //   registerSuccessTest();
            loginSuccessTest();
            string searchResult3 = userService.searchByCategory("pets");
            Assert.IsTrue(searchResult3.Equals("[]"));
        }

        [TestMethod]
        public void searchByNameSucc()
        {
            string searchResult1 = userService.searchByName("stove");
            int next = DBStore.nextStoreID;
            Assert.AreEqual(searchResult1, "[{\"productID\":2,\"productName\":" +
                "\"stove\",\"productCategory\":\"kitchen\",\"price\":200,\"rank\":3" +
                ",\"quantityLeft\":2,\"storeID\":" +
                (next - 1) + "}]");
            //Assert.IsTrue(productExists("stove", searchResult1));
        }

        [TestMethod]
        public void searchByNameFail()
        {
            string searchResult2 = userService.searchByName("toaster");
            Assert.IsTrue(searchResult2.Equals("[]"));
        }

        public bool productExists(String name, List<Product> products)
        {
            foreach (Product p in products)
            {
                if (Equals(p.getProductName(), name))
                    return true;
            }
            return false;
        }

        //2.8.1 
        [TestMethod]
        public void purchaseTestWithPolicySuccess()
        {
            try
            {
                //registerSuccessTest();
                loginSuccessTest();
                int store1 = userService.createStore(session2, "Zoo Land", "pets");
                int product1 = storeService.addProduct("dogs", "big dogs", 80, 2, 4, store1, session2);
                int product2 = storeService.addProduct("cats", "grey cats", 110, 5, 10, store1, session2);

                basketService.addToCart(session2, product1, 2);
                basketService.addToCart(session2, product2, 3);

                storeService.addMinAmountPolicy(store1, session2, 2);
                userService.checkBasket(session2);
                userService.purchaseBasket(session2, "HaJelmonit 14", "234", "", "", "", "");
                Assert.IsTrue(true);
                Assert.IsTrue(storeService.getProductQuantityLeft(product1) == 2);
                Assert.IsTrue(storeService.getProductQuantityLeft(product2) == 7);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void purchaseTestWithPolicyFail()
        {
            try
            {
                //registerSuccessTest();
                loginSuccessTest();

                int store1 = userService.createStore(session2, "Zoo Land", "pets");
                int product1 = storeService.addProduct("dogs", "big dogs", 80, 2, 4, store1, session2);

                basketService.addToCart(session2, product1, 3);
                basketService.changeQuantity(session2, product1, 7);
                userService.purchaseBasket(session2, "Neviot 22", "123", "", "", "", "");
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }
        //2.8.2
        [TestMethod]
        public void purchaseTestWithDiscountSuccess()
        {
            try
            {
                loginSuccessTest();
                int store1 = userService.createStore(session2, "Zoo Land", "pets");
                int product1 = storeService.addProduct("dogs", "big dogs", 80, 2, 4, store1, session2);

                basketService.addToCart(session2, product1, 2);

                storeService.addReliantDiscountSameProduct(store1, session2, 0.3, "12/12/2020", 1, product1);
                double price = basketService.getActualTotalPrice(session2);
                Assert.IsTrue(160 * 0.7 == price);
                userService.checkBasket(session2);
                userService.purchaseBasket(session2, "HaJelmonit 14", "234", "", "", "", "");
                Assert.IsTrue(true);

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
        //2.8.2
        [TestMethod]
        public void purchaseTestWithDiscountFail()
        {
            try
            {
                loginSuccessTest();
                int store1 = userService.createStore(session2, "Zoo Land", "pets");
                int product1 = storeService.addProduct("dogs", "big dogs", 80, 2, 4, store1, session2);

                basketService.addToCart(session2, product1, 2);

                storeService.addReliantDiscountSameProduct(store1, session2, 0.3, "12/12/2020", 5, product1);
                double price = basketService.getActualTotalPrice(session2);
                Assert.IsTrue(160 == price);
                userService.checkBasket(session2);
                userService.purchaseBasket(session2, "HaJelmonit 14", "234", "", "", "", "");
                Assert.IsTrue(true);

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
        //3.1
        [TestMethod]
        public void logoutTestSuccess()
        {
            try
            {
                // registerSuccessTest();
                loginSuccessTest();

                userService.logout(session2);
                //???do we need to check
                // Assert.IsTrue(session.getState() is Guest);
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void logoutTestFail()
        {
            try
            {
                userService.logout(session2);
                Assert.Fail();
            }
            catch (UserStateException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {

            }
        }

        //3.2
        [TestMethod()]
        public void createStoreBySubscribedUserSuccessTest()
        {
            try
            {
                loginSuccessTest();
                int store = userService.createStore(session2, "Apple", "apples");
                Assert.IsTrue(true);

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
        [TestMethod()]
        public void createStoreBySubscribedUserFailTest()
        {
            try
            {
                loginSuccessTest();
                int store = userService.createStore(session2, "", "apples");
                Assert.Fail();


            }
            catch (IllegalNameException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        //6.2
        [TestMethod]
        public void removeSubscribedUserTest()
        {

            //Session sessionOwner = userService.startSession();
            //Session sessionManager = userService.startSession();
            //SubscribedUser subscribedToDelete = sessionManager.getSubscribedUser();
            //userService.register(sessionOwner, "bob", "theOwner");
            //userService.register(sessionManager, "rob", "theManager");
            //userService.login(sessionOwner, "bob", "theOwner");
            //Store store1 = userService.createStore(sessionOwner, "Zara", "clothes");
            //Store store2 = userService.createStore(sessionOwner, "Urban", "clothes");
            //StoreRole owner1 = sessionOwner.getSubscribedUser().getStoreRole(store1);
            //owner1.addManager(sessionManager.getSubscribedUser(), new Permissions(true, true, true));
            //StoreRole owner2 = sessionOwner.getSubscribedUser().getStoreRole(store2);
            //owner2.addOwner(sessionManager.getSubscribedUser());

            //Session sessionAdmin = userService.startSession();
            //Assert.AreEqual(userService.login(sessionAdmin, "admin", "1234"), "");
            //Assert.IsTrue(Equals(userService.removeUser(sessionAdmin, "rob"),""));

            //Assert.IsTrue(sessionManager.getState() is Guest);
            //Assert.IsTrue(store1.getStoreRole(subscribedToDelete) == null);
            //Assert.IsTrue(store2.getStoreRole(subscribedToDelete) == null);
            //Assert.IsTrue(sessionManager.getSubscribedUser() == null);

            ////user does not exist anymore, login fails
            //Assert.AreNotEqual(userService.login(sessionAdmin, "rob", "theManager"), "");

            ////alternatives
            //Assert.IsFalse(Equals(userService.removeUser(sessionAdmin, "haim"),""));
            //Assert.IsFalse(Equals(userService.removeUser(sessionAdmin, "admin"), ""));
            //Assert.IsFalse(Equals(userService.removeUser(sessionManager, "rob"), ""));
            //Assert.IsFalse(Equals(userService.removeUser(sessionOwner, "rob"), ""));

        }
    }

}



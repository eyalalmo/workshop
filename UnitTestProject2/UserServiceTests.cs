


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
        private int session1, session2,storeId; // session3;



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
        //2.2+2.3
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

        //2.8
        [TestMethod]
        public void purchaseTestSuccess()
        {
            try
            {
                //registerSuccessTest();
                loginSuccessTest();
                int store1 = userService.createStore(session2, "Zoo Land", "pets");
                int product1 = storeService.addProduct("dogs", "big dogs", 80, 2, 4, store1, session2);
                int product2 = storeService.addProduct("cats", "grey cats", 110, 5, 10, store1, session2);

                int store2 = userService.createStore(session2, "Shufersal", "food");
                int product3 = storeService.addProduct("milk", "3%", 65, 2, 1, store2, session2);
                int product4 = storeService.addProduct("water", "blue water", 70, 4, 4, store2, session2);

                basketService.addToCart(session2, product1, 2);
                basketService.addToCart(session2, product2, 3);
                basketService.addToCart(session2, product3, 1);
                basketService.addToCart(session2, product4, 3);

                userService.purchaseBasket(session2, "HaJelmonit 14", "234", "", "", "", "");
                Assert.IsTrue(true);

                //???????????????do we need to check state
                // Assert.IsTrue(product1.getQuantityLeft() == 2);
                // Assert.IsTrue(product2.getQuantityLeft() == 7);
                // Assert.IsTrue(product3.getQuantityLeft() == 0);
                // Assert.IsTrue(product4.getQuantityLeft() == 1);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void purchaseTestFail()
        {
            try
            {
                //registerSuccessTest();
                loginSuccessTest();

                int store1 = userService.createStore(session2, "Zoo Land", "pets");
                int product1 = storeService.addProduct("dogs", "big dogs", 80, 2, 4, store1, session2);

                basketService.addToCart(session2, product1, 6);
                userService.purchaseBasket(session2, "Neviot 22", "123", "", "", "", "");
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
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
                storeId = userService.createStore(session2, "Apple", "apples");
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
            int sessionManager = userService.startSession();
            try
            {
                createStoreBySubscribedUserSuccessTest();// owner = user,user session2
                userService.register(sessionManager, "rob", "theManager");
                int store2 = userService.createStore(session2, "Urban", "clothes");
                storeService.addManager(storeId, "rob", true, true, true, session2);
                storeService.addOwner(store2, "rob", session2);
                int sessionAdmin = userService.startSession();
                userService.login(sessionAdmin, "u1", "123");
                userService.removeUser(sessionAdmin, "rob");
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
            try
            {
                userService.login(sessionManager, "rob", "theManager");
                Assert.Fail();
            }
            catch (LoginException)
            {

            }
        }
    }

}



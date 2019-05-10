


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
    /*
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
            try
            {
                userService.setup();

                session1 = userService.startSession();
                session2 = userService.startSession();
                // session3 = userService.startSession();

                userService.register(session1, "zubu", "mafu");
                userService.login(session1, "zubu", "mafu");

                int store1 = userService.createStore(session1, "Golf", "Clothes");
                int store2 = userService.createStore(session1, "Shiomi", "Technology");

                //storeService.addManager(store1, "dani1", true, true, true, session);

                storeService.addProduct("shirt", "clothes", 20, 5, 2, store1, session1);
                storeService.addProduct("pan", "kitchen", 100, 2, 4, store1, session1);
                storeService.addProduct("stove", "kitchen", 200, 3, 2, store1, session1);
                storeService.addProduct("pants", "clothes", 120, 1, 2, store2, session1);
                storeService.addProduct("socks", "clothes", 110, 4, 2, store2, session1);
            }
            catch (Exception)
            {
                throw new ExecutionEngineException();
            }
        }
        //2.2+2.3
        [TestMethod]
        public void registerSuccessTest()
        {
            try
            {
                userService.register(session2, "user", "user");
            }catch(Exception)
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

        //Assert.AreEqual(str, "");
        //Assert.AreEqual(userService.login(session, "user", "user"), "");
        //Assert.IsTrue(session.getSubscribedUser() != null);
        //Assert.IsTrue(session.getState() is LoggedIn);

        //Assert.AreNotEqual(userService.login(session, "user", "user"), "");
        //Assert.IsTrue(session.getState() is LoggedIn);
        //Assert.AreNotEqual(userService.login(session, "bbb", "aaaa"), "");
        //Assert.AreNotEqual(userService.register(session, "user", "user"), "");
        //2.5.1+2.5.2
       
        [TestMethod]
        public void searchByCategoryTest1()
        {
            //registerSuccessTest();
            loginSuccessTest();
            List<Product> searchResult1 = userService.searchProducts(null, null, "kitchen");
            Assert.IsTrue(searchResult1.Count == 2);
            //Assert.IsTrue(productExists("pan", searchResult1));
            //Assert.IsTrue(productExists("stove", searchResult1));
        }

        [TestMethod]
        public void searchByCategoryTest2()
        {
            //registerSuccessTest();
            loginSuccessTest();
            List<Product> searchResult2 = userService.searchProducts(null, null, "clothes");
            Assert.IsTrue(searchResult2.Count == 3);
            //Assert.IsTrue(productExists("shirt", searchResult2));
            //Assert.IsTrue(productExists("pants", searchResult2));
            //Assert.IsTrue(productExists("socks", searchResult2));

        }
        [TestMethod]
        public void searchByCategoryFail()
        {
         //   registerSuccessTest();
            loginSuccessTest();
            List<Product> searchResult3 = userService.searchProducts(null, null, "pets");
            Assert.IsTrue(searchResult3.Count == 0);
        }

        [TestMethod]
        public void searchByNameSucc()
        {
            List<Product> searchResult1 = userService.searchProducts("stove", null, null);
            Assert.IsTrue(searchResult1.Count == 1);
            //Assert.IsTrue(productExists("stove", searchResult1));

        }

        [TestMethod]
        public void searchByNameFail()
        {
            List<Product> searchResult2 = userService.searchProducts("toaster", null, null);
            Assert.IsTrue(searchResult2.Count == 0);
        }
        [TestMethod]
        public void filterTest1()
        {
            
            List<Product> searchResult2 = userService.searchProducts(null, null, "clothes");
            Assert.IsTrue(searchResult2.Count == 3);
            //Assert.IsTrue(productExists("shirt", searchResult2));
            //Assert.IsTrue(productExists("pants", searchResult2));
            //Assert.IsTrue(productExists("socks", searchResult2));
        }
        public void filterTest2()
        {
            List<Product> searchResult1 = userService.searchProducts(null, null, "kitchen");
            int[] arr1 = { 60, 120 };
            List<Product> filterResult1 = userService.filterProducts(searchResult1, arr1, 0);
            Assert.IsTrue(filterResult1.Count == 1);
            //Assert.IsTrue(productExists("pan", filterResult1));
        }
        public void filterTest3()
        {
            List<Product> searchResult2 = userService.searchProducts(null, null, "clothes");
            int[] arr3 = { 10, 40 };
            List<Product> filterResult3 = userService.filterProducts(searchResult2, arr3, 5);
            Assert.IsTrue(filterResult3.Count == 1);
        }
        public void filterTestFail()
        {
            List<Product> searchResult2 = userService.searchProducts(null, null, "clothes");
            int[] arr2 = { -40, -20 };
            List<Product> filterResult2 = userService.filterProducts(searchResult2, arr2, 0);
            Assert.IsTrue(filterResult2.Count == 0);


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

                userService.purchaseBasket(session2);
                Assert.IsTrue(true);

                //???????????????do we need to check state
               // Assert.IsTrue(product1.getQuantityLeft() == 2);
                // Assert.IsTrue(product2.getQuantityLeft() == 7);
                // Assert.IsTrue(product3.getQuantityLeft() == 0);
                // Assert.IsTrue(product4.getQuantityLeft() == 1);
            }
            catch (Exception)
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
                userService.purchaseBasket(session2);
                Assert.Fail();
            }
            catch (AlreadyExistException)
            {
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
                //userService.register(session, "anna", "banana");
                //userService.login(session, "anna", "banana");
                //SubscribedUser user = session.getSubscribedUser();
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
              //  registerSuccessTest();
                loginSuccessTest();
                //userService.register(session, "anna", "banana");
                //userService.login(session, "anna", "banana");
                //SubscribedUser user = session.getSubscribedUser();
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
    */
}



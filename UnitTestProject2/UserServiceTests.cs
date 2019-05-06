


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
        private UserService userService;
        private StoreService storeService;
        private BasketService basketService;
        private int session;

        [TestInitialize()]
        public void Initialize()
        {
            userService = UserService.getInstance();
            storeService = StoreService.getInstance();
            basketService = BasketService.getInstance();
            session = userService.startSession();
            DBStore.getInstance().init();
            DBSubscribedUser.getInstance().init();
            DBProduct.getInstance().initDB();
            int store1 = userService.createStore(session, "Golf", "Clothes");
            int store2 = userService.createStore(session, "Shiomi", "Technology");
            storeService.addProduct("shirt", "clothes", 20, 5, 2, store1, session);
            storeService.addProduct("pan", "kitchen", 100, 2, 4, store1, session);
            storeService.addProduct("stove", "kitchen", 200, 3, 2, store1, session);
            storeService.addProduct("pants", "clothes", 120, 1, 2, store2, session);
            storeService.addProduct("socks", "clothes", 110, 4, 2, store2, session);

        }
        //2.2+2.3
        [TestMethod]
        public void registerSuccessTest()
        {
            try
            {
                userService.register(session, "user", "user");
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
                userService.register(session, "user", "user");
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
                userService.login(session, "user", "user");
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
                userService.login(session, "user", "user33");
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
                userService.login(session, "user", "user");
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
            registerSuccessTest();
            loginSuccessTest();
            List<Product> searchResult1 = userService.searchProducts(null, null, "kitchen");
            Assert.IsTrue(searchResult1.Count == 2);
            //Assert.IsTrue(productExists("pan", searchResult1));
            //Assert.IsTrue(productExists("stove", searchResult1));
        }

        [TestMethod]
        public void searchByCategoryTest2()
        {
            registerSuccessTest();
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
            registerSuccessTest();
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
                registerSuccessTest();
                loginSuccessTest();
                int store1 = userService.createStore(session, "Zoo Land", "pets");
                int product1 = storeService.addProduct("dogs", "big dogs", 80, 2, 4, store1, session);
                int product2 = storeService.addProduct("cats", "grey cats", 110, 5, 10, store1, session);

                int store2 = userService.createStore(session, "Shufersal", "food");
                int product3 = storeService.addProduct("milk", "3%", 65, 2, 1, store2, session);
                int product4 = storeService.addProduct("water", "blue water", 70, 4, 4, store2, session);

                basketService.addToCart(session, product1, 2);
                basketService.addToCart(session, product2, 3);
                basketService.addToCart(session, product3, 1);
                basketService.addToCart(session, product4, 3);

                userService.purchaseBasket(session);
                Assert.IsTrue(true);

                //???????????????do we need to check state
                /* Assert.IsTrue(product1.getQuantityLeft() == 2);
                 Assert.IsTrue(product2.getQuantityLeft() == 7);
                 Assert.IsTrue(product3.getQuantityLeft() == 0);
                 Assert.IsTrue(product4.getQuantityLeft() == 1);*/
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
                registerSuccessTest();
                loginSuccessTest();

                int store1 = userService.createStore(session, "Zoo Land", "pets");
                int product1 = storeService.addProduct("dogs", "big dogs", 80, 2, 4, store1, session);

                basketService.addToCart(session, product1, 6);
                userService.purchaseBasket(session);
                Assert.Fail();
            }
            catch (IllegalAmountException)
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
                registerSuccessTest();
                loginSuccessTest();

                userService.logout(session);
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
                userService.logout(session);
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
                registerSuccessTest();
                loginSuccessTest();
                //userService.register(session, "anna", "banana");
                //userService.login(session, "anna", "banana");
                //SubscribedUser user = session.getSubscribedUser();
                int store = userService.createStore(session, "Apple", "apples");
                Assert.IsTrue(true);
                /*List<StoreRole> roles = store.getRoles();
                Assert.IsTrue(roles.Count == 1);
                StoreRole role = roles[0];
                Assert.IsTrue(role is StoreOwner);
                Assert.IsTrue(Equals(role.getUser(), user));

                List<StoreRole> userRoles = user.getStoreRoles();
                Assert.IsTrue(userRoles.Contains(role));

                Assert.AreEqual(userService.createStore(null, "", ""), null);
                */
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
                registerSuccessTest();
                loginSuccessTest();
                //userService.register(session, "anna", "banana");
                //userService.login(session, "anna", "banana");
                //SubscribedUser user = session.getSubscribedUser();
                int store = userService.createStore(session, "", "apples");
                Assert.Fail();
                /*List<StoreRole> roles = store.getRoles();
                Assert.IsTrue(roles.Count == 1);
                StoreRole role = roles[0];
                Assert.IsTrue(role is StoreOwner);
                Assert.IsTrue(Equals(role.getUser(), user));

                List<StoreRole> userRoles = user.getStoreRoles();
                Assert.IsTrue(userRoles.Contains(role));

                Assert.AreEqual(userService.createStore(null, "", ""), null);
                */

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



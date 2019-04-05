


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
        private Session session;
        [TestInitialize()]
        public void Initialize()
        {
            userService = UserService.getInstance(); 
            session = userService.startSession();
            DBStore.getInstance().init();
            DBSubscribedUser.getInstance().cleanDB();
        }
        //2.2+2.3
        [TestMethod]
        public void registerLoginTest()
        {
            String str = userService.register(session, "user", "user");
            Assert.AreEqual(str, "");
            Assert.AreEqual(userService.login(session, "user", "user"), "");
            Assert.IsTrue(session.getSubscribedUser() != null);
            Assert.IsTrue(session.getState() is LoggedIn);

            Assert.AreNotEqual(userService.login(session, "user", "user"), "");
            Assert.IsTrue(session.getState() is LoggedIn);
            Assert.AreNotEqual(userService.login(session, "bbb", "aaaa"), "");
            Assert.AreNotEqual(userService.register(session, "user", "user"), "");

        }

        //2.5.1+2.5.2
        [TestMethod]
        public void initSearchAndFilterTests()
        {
            userService = UserService.getInstance();
            StoreService storeService = StoreService.getInstance();
            session = userService.startSession();

            userService.register(session, "gal", "1111");
            userService.login(session, "gal", "1111");

            Store store1 = userService.createStore(session, "Golf", "Clothes");
            Store store2 = userService.createStore(session, "Shiomi", "Technology");
            storeService.addProduct("shirt", "clothing", 20, 5, 2, store1, session);
            storeService.addProduct("pan", "kitchen", 100, 2, 4, store1, session);
            storeService.addProduct("stove", "kitchen", 200, 3, 2, store1, session);
            storeService.addProduct("pants", "clothing", 120, 1, 2, store2, session);
            storeService.addProduct("socks", "clothing", 110, 4, 2, store2, session);

        }
        [TestMethod]
        public void searchByCategoryTest()
        {

            List<Product> searchResult1 = userService.searchProducts(null, null, "kitchen");
            Assert.IsTrue(searchResult1.Count == 2);
            Assert.IsTrue(productExists("pan", searchResult1));
            Assert.IsTrue(productExists("stove", searchResult1));


            List<Product> searchResult2 = userService.searchProducts(null, null, "clothing");
            Assert.IsTrue(searchResult2.Count == 3);
            Assert.IsTrue(productExists("shirt", searchResult2));
            Assert.IsTrue(productExists("pants", searchResult2));
            Assert.IsTrue(productExists("socks", searchResult2));

            List<Product> searchResult3 = userService.searchProducts(null, null, "pets");
            Assert.IsTrue(searchResult3.Count == 0);


        }

        [TestMethod]
        public void searchByName()
        {

            List<Product> searchResult1 = userService.searchProducts("stove", null, null);
            Assert.IsTrue(searchResult1.Count == 1);
            Assert.IsTrue(productExists("stove", searchResult1));

            List<Product> searchResult2 = userService.searchProducts("toaster", null, null);
            Assert.IsTrue(searchResult2.Count == 0);

        }
    
        [TestMethod]
        public void filterTest()
        {
            
            List<Product> searchResult1 = userService.searchProducts(null, null, "kitchen");
            List<Product> searchResult2 = userService.searchProducts(null, null, "clothing");
            Assert.IsTrue(searchResult2.Count == 3);
            Assert.IsTrue(productExists("shirt", searchResult2));
            Assert.IsTrue(productExists("pants", searchResult2));
            Assert.IsTrue(productExists("socks", searchResult2));

            int[] arr1 = { 60, 120 };
            List<Product> filterResult1 = userService.filterProducts(searchResult1,arr1, 0);
            Assert.IsTrue(filterResult1.Count == 1);
            Assert.IsTrue(productExists("pan", filterResult1));

            int[] arr3 = { 10, 40 };
            List<Product> filterResult3 = userService.filterProducts(searchResult2, arr3, 5);
            Assert.IsTrue(filterResult3.Count == 1);

            int[] arr2 = { -40, -20 };
            List<Product> filterResult2 = userService.filterProducts(searchResult1, arr2, 0);
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

        //3.1
        [TestMethod]
        public void logoutTest()
        {
            session = userService.startSession();
            userService.register(session, "bar", "bur");
            userService.login(session, "bar", "bur");
            Assert.IsTrue(session.getState() is LoggedIn);
            userService.logout(session);
            Assert.IsTrue(session.getState() is Guest);
        }

        //3.2
        [TestMethod()]
        public void createStoreBySubscribedUserTest()
        {

            UserService userService = UserService.getInstance();
            session = userService.startSession();

            userService.register(session, "anna", "banana");
            userService.login(session, "anna", "banana");
            SubscribedUser user = session.getSubscribedUser();
            Store store = userService.createStore(session, "Apple", "apples");
            List<StoreRole> roles = store.getRoles();
            Assert.IsTrue(roles.Count == 1);
            StoreRole role = roles[0];
            Assert.IsTrue(role is StoreOwner);
            Assert.IsTrue(Equals(role.getUser(), user));

            List<StoreRole> userRoles = user.getStoreRoles();
            Assert.IsTrue(userRoles.Contains(role));

            Assert.AreEqual(userService.createStore(null, "", ""), null);


        }


        //6.2
        [TestMethod]
        public void removeSubscribedUserTest()
        {

            Session sessionOwner = userService.startSession();
            Session sessionManager = userService.startSession();
            SubscribedUser subscribedToDelete = sessionManager.getSubscribedUser();
            userService.register(sessionOwner, "bob", "theOwner");
            userService.register(sessionManager, "rob", "theManager");
            userService.login(sessionOwner, "bob", "theOwner");
            Store store1 = userService.createStore(sessionOwner, "Zara", "clothing");
            Store store2 = userService.createStore(sessionOwner, "Urban", "clothing");
            StoreRole owner1 = sessionOwner.getSubscribedUser().getStoreRole(store1);
            owner1.addManager(sessionManager.getSubscribedUser(), new Permissions(true, true, true));
            StoreRole owner2 = sessionOwner.getSubscribedUser().getStoreRole(store2);
            owner2.addOwner(sessionManager.getSubscribedUser());

            Session sessionAdmin = userService.startSession();
            Assert.AreEqual(userService.login(sessionAdmin, "admin", "1234"), "");
            Assert.IsTrue(Equals(userService.removeUser(sessionAdmin, "rob"),""));
            
            Assert.IsTrue(sessionManager.getState() is Guest);
            Assert.IsTrue(store1.getStoreRole(subscribedToDelete) == null);
            Assert.IsTrue(store2.getStoreRole(subscribedToDelete) == null);
            Assert.IsTrue(sessionManager.getSubscribedUser() == null);

            //user does not exist anymore, login fails
            Assert.AreNotEqual(userService.login(sessionAdmin, "rob", "theManager"), "");

            //alternatives
            Assert.IsFalse(Equals(userService.removeUser(sessionAdmin, "haim"),""));
            Assert.IsFalse(Equals(userService.removeUser(sessionAdmin, "admin"), ""));
            Assert.IsFalse(Equals(userService.removeUser(sessionManager, "rob"), ""));
            Assert.IsFalse(Equals(userService.removeUser(sessionOwner, "rob"), ""));

        }
    }
}



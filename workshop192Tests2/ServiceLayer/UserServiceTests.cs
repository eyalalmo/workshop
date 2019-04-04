


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
        //2.2+2.3
        [TestMethod]
        public void TestMethod1()
        {
            UserService user = UserService.getInstance();
            Session session = user.startSession();
            String str = user.register(session, "user", "user");
            Assert.AreEqual(str, "");
            Assert.AreEqual(user.login(session, "user", "user"), "");
            Assert.IsTrue(session.getSubscribedUser() != null);
            Assert.IsTrue(session.getState() is LoggedIn);
            //alternative
            Assert.AreNotEqual(user.login(session, "user", "user"), "");
            Assert.IsTrue(session.getState() is LoggedIn);
            Assert.AreNotEqual(user.login(session, "bbb", "aaaa"), "");
            Assert.AreNotEqual(user.register(session, "user", "user"), "");
        }


        //2.5.1+2.5.2
        [TestMethod]
        public void TestMethod2()
        {
            UserService userService = UserService.getInstance();
            StoreService storeService = StoreService.getInstance();
            Session session = userService.startSession();
            Store store1 = storeService.addStore();
            Store store2 = storeService.addStore();
            storeService.addProduct("shirt", "clothing", 20, 5, 2, store1, session);
            storeService.addProduct("pan", "kitchen", 100, 2, 4, store1, session);
            storeService.addProduct("stove", "kitchen", 200, 3, 2, store1, session);
            storeService.addProduct("pants", "clothing", 120, 1, 2, store2, session);
            storeService.addProduct("socks", "clothing", 110, 4, 2, store2, session);


            List<Product> searchResult1 = userService.searchProducts(null, null, "kitchen");
            Assert.IsTrue(searchResult1.Count == 2);
            Assert.IsTrue(productExists("pan", searchResult1));
            Assert.IsTrue(productExists("stove", searchResult1));


            List<Product> searchResult2 = userService.searchProducts("shirt", null, null);
            Assert.IsTrue(searchResult2.Count == 3);
            Assert.IsTrue(productExists("shirt", searchResult2));
            Assert.IsTrue(productExists("pants", searchResult2));
            Assert.IsTrue(productExists("socks", searchResult2));


            List<Product> searchResult3 = userService.searchProducts("skirt", null, null);
            Assert.IsTrue(searchResult3.Count == 0);


            List<Product> searchResult4 = userService.searchProducts("skirt", null, "22222");
            Assert.IsTrue(searchResult3.Count == 0);


            List<Product> filterResult1 = userService.filterProducts(searchResult1, [60, 120], 0);
            Assert.IsTrue(filterResult1.Count == 1);
            Assert.IsTrue(productExists("pan", filterResult1));


            List<Product> filterResult2 = userService.filterProducts(searchResult1, [-20, -40], 0);
            Assert.IsTrue(filterResult2.Count == 0);


            List<Product> filterResult3 = userService.filterProducts(searchResult2, [10, 40], 5);
            Assert.IsTrue(filterResult3.Count == 0);


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
        public void TestMethod3()
        {
            UserService user = UserService.getInstance();
            Session session = new Session();
            user.register(session, "bar", "bur");
            user.login(session, "bar", "bur");
            Assert.IsTrue(session.getState() is LoggedIn);
            user.logout(session);
            Assert.IsTrue(session.getState() is Guest);
        }

        //3.2
        [TestMethod()]
        public void TestMethod4()
        {
         
            UserService userService = UserService.getInstance();
            Session session = new Session();

            userService.register(session, "anna", "banana");
            userService.login(session, "anna", "banana");
            Store store = 
        }

        //6.2
        [TestMethod]
        public void TestMethod5()
        {
            UserService user = UserService.getInstance();
            Session session1 = user.startSession();
            user.register(session1, "bob", "theBuilder");
            user.login(session1, "bob", "theBuilder");
            Store store = user.createStore(session1, "Zara", "clothing");
            user.logout(session1);

            Session session2 = user.startSession();
            Assert.AreEqual(user.login(session2, "admin", "1234"), "");
            user.removeUser(session2, "bob");

            //user does not exist anymore, login fails
            Assert.AreNotEqual(user.login(session1, "bob", "theBuilder"), "");
            Assert.IsFalse(store.isActive());

            ////////////////////////////////////////

        }
    }
}


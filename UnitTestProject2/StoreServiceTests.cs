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
    public class StoreServiceTests
    {
        private StoreService storeService;
        private UserService userService;

        [TestInitialize()]
        public void TestInitialize()
        {
            storeService = StoreService.getInstance();
            userService = UserService.getInstance();
            DBStore.getInstance().init();
            DBSubscribedUser.getInstance().cleanDB();
        }
        //4.1.1+4.1.2+4.1.3
        [TestMethod()]
        public void addRemoveEditProductTest()
        {
            //add product

            Session session = new Session();

            userService.register(session, "bananaMan", "bbbb");
            userService.login(session, "bananaMan", "bbbb");
            Store store = userService.createStore(session, "Bananas", "all types of bananas");

            Product product1 = storeService.addProduct("banana1", "green bananas", 100, 2, 6, store, session);
            Product product2 = storeService.addProduct("banana2", "pink bananas", 90, 5, 10, store, session);

            LinkedList<Product> products = store.getProductList();
            Assert.IsTrue(products.Count == 2);
            Assert.IsTrue(store.productExists(product1.getProductID()));
            Assert.IsTrue(store.productExists(product2.getProductID()));

            Assert.AreNotEqual(storeService.addProduct("banana2", "pink bananas", 90, 10, -1, store, session), "");
            Assert.AreNotEqual(storeService.addProduct("", "", 90, 10, 2, store, session), "");

            //edit product
            Assert.IsTrue(Equals(storeService.addToProductQuantity(product1, 3, session), ""));
            Assert.IsTrue(Equals(storeService.decFromProductQuantity(product1, 5, session), ""));
            Assert.IsFalse(Equals(storeService.addToProductQuantity(product1, -2, session), ""));

            //remove product
            Assert.IsTrue(Equals(storeService.removeProduct(product1, session), ""));
            Assert.IsTrue(Equals(storeService.removeProduct(product2, session), ""));
            Assert.IsTrue(store.getProductList().Count == 0);

            Assert.IsFalse(Equals(storeService.removeProduct(null, session), ""));

        }

        //4.3
        [TestMethod()]
        public void addOwnerByAnOwnerSuccTest()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();

            userService.register(session1, "anna", "banana");
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani", "123");
            userService.login(session2, "dani", "123");

            userService.register(session3, "bar", "123");
            userService.login(session3, "bar", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreEqual(storeService.addOwner(store, "dani", session1), "");
            Product product = storeService.addProduct("myProduct", "some category", 10, 0, 10, store, session2);
            Assert.AreNotEqual(product, null);
            Assert.AreEqual(storeService.addToProductQuantity(product, 10, session2), "");
            Assert.AreEqual(storeService.decFromProductQuantity(product, 10, session2), "");
            Assert.AreEqual(storeService.setProductDiscount(product, null, session2), "");
            Assert.AreEqual(storeService.removeProduct(product, session2), "");

            Assert.AreEqual(storeService.addOwner(store, "bar", session2), "");
            product = storeService.addProduct("myProduct2", "some category", 10, 0, 10, store, session3);
            Assert.AreNotEqual(product, null);
            Assert.AreEqual(storeService.addToProductQuantity(product, 10, session3), "");
            Assert.AreEqual(storeService.decFromProductQuantity(product, 10, session3), "");
            Assert.AreEqual(storeService.setProductDiscount(product, null, session3), "");
            Assert.AreEqual(storeService.removeProduct(product, session3), "");
        }

        //4.3
        [TestMethod()]
        public void addOwnerByAnOwnerFailTest()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();

            userService.register(session1, "anna", "banana");
            userService.login(session1, "anna", "banana");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreNotEqual(storeService.addOwner(store, "dani", session1), "");
        }

        //4.5
        [TestMethod()]
        public void addOwnerByAnOwnerFailTest2()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();

            userService.register(session1, "anna", "banana");
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            storeService.addOwner(store, "dani", session1);
            Assert.AreNotEqual(storeService.addOwner(store, "dani", session1), "");
        }

        // 4.4
        [TestMethod()]
        public void removeOwnerTest()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();
            Session session4 = new Session();

            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            userService.register(session3, "dani2", "123");
            userService.register(session4, "dani3", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreEqual("", storeService.addOwner(store, "dani1", session1));
            Assert.AreEqual("", storeService.addOwner(store, "dani2", session2));
            Assert.AreEqual("", storeService.addOwner(store, "dani3", session2));
            Assert.AreEqual(storeService.removeRole(store, "dani1", session1), "");
            foreach (StoreRole role in store.getRoles())
            {
                if (role.getAppointedBy() == session1.getSubscribedUser())
                {
                    Assert.Fail();
                }
            }

        }

        // 4.4
        [TestMethod()]
        public void removeOwnerTestFaild2()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();
            Session session4 = new Session();

            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            userService.register(session3, "dani2", "123");
            userService.register(session4, "dani3", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreEqual("", storeService.addOwner(store, "dani1", session1));
            Assert.AreEqual("", storeService.addOwner(store, "dani2", session2));
            Assert.AreNotEqual(storeService.removeRole(store, "dani3", session2), "");
        }

        //4.5
        [TestMethod()]
        public void addMannagerByAnOwner1()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();

            userService.register(session1, "anna", "banana");
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani", "123");
            userService.login(session2, "dani", "123");

            userService.register(session3, "yaniv", "123");
            userService.login(session3, "yaniv", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreEqual(storeService.addManager(store, "dani", true, true, true, session1), "");
            Product product = storeService.addProduct("myProduct", "some category", 10, 0, 10, store, session2);
            Assert.AreNotEqual(product, null);
            Assert.AreEqual(storeService.addToProductQuantity(product, 10, session2), "");
            Assert.AreEqual(storeService.decFromProductQuantity(product, 10, session2), "");
            Assert.AreEqual(storeService.setProductDiscount(product, null, session2), "");

            Assert.AreEqual(storeService.addManager(store, "yaniv", false, false, false, session1), "");
            Assert.AreNotEqual(storeService.addToProductQuantity(product, 10, session3), "");
            Assert.AreNotEqual(storeService.decFromProductQuantity(product, 10, session3), "");
            Assert.AreNotEqual(storeService.setProductDiscount(product, null, session3), "");
        }

        //4.5
        [TestMethod()]
        public void addMannagerByAnOwner2()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();

            userService.register(session1, "anna", "banana");
            userService.login(session1, "anna", "banana");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreNotEqual(storeService.addManager(store, "dani", true, true, true, session1), "");
        }

        //4.5
        [TestMethod()]
        public void addMannagerByAnOwner3()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();

            userService.register(session1, "anna", "banana");
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            storeService.addManager(store, "dani", true, true, true, session1);
            Assert.AreNotEqual(storeService.addManager(store, "dani", true, true, true, session1), "");

        }
        
        // 4.6
        [TestMethod()]
        public void removeManagerTest()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();

            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreEqual("", storeService.addManager(store, "dani1", true, true, true, session1));
            Assert.AreEqual(storeService.removeRole(store, "dani1", session1), "");
        }

        // 4.6
        [TestMethod()]
        public void removeManagerTestFaild1()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();
            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            userService.register(session3, "dani2", "123");
            userService.login(session3, "dani2", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);

            Assert.AreEqual("", storeService.addOwner(store, "dani2", session1));

            Assert.AreEqual("", storeService.addManager(store, "dani1", true, true, true, session1));
            Assert.AreNotEqual(storeService.removeRole(store, "dani1", session3), "");
        }
        // 4.6
        [TestMethod()]
        public void removeManagerTestFaild2()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();
            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");
            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreNotEqual(storeService.removeRole(store, "dani1", session1), "");
        }
    }
}
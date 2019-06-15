using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Bridge;

namespace workshop192.Domain.Tests
{
    [TestClass()]
    public class StoreOwnerTests
    {
        Session session1, session2, session3;
       
        Store store;
        StoreRole sr;
        Permissions per;
        Product p, p1;
        [TestInitialize()]
        public void TestInitialize()
        {
            MarketSystem.initTestWitOutRead();
            session1 = new Session();
            string pass = DomainBridge.getInstance().encryptPassword("123");
            session1.register("eyal", pass);

            session2 = new Session();
            session2.register("bar", pass);

            session3 = new Session();
            session3.register("etay", pass);

            session1.login("eyal", "123");
            session2.login("bar", "123");
            session3.login("etay", "123");

            store = session1.createStore("mystore", "a store");
            sr = session1.getSubscribedUser().getStoreRole(store);
            p = new Product("product", "cat", 10, 0, 10, store);
            p1 = new Product("product1", "cat", 10, 0, 10, store);

            per = new Permissions(true, true, true);
        }

        [TestMethod()]
        public void addProductSuccTest()
        {
            try
            {
                sr.addProduct(p);

                Product returnP = DBProduct.getInstance().getProductByID(p.getProductID());

                Assert.AreEqual(returnP, p);
                Assert.AreEqual(store.getProductList().Contains(p), true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        
        [TestMethod()]
        public void removeProductSuccTest()
        {
            try
            {
                addProductSuccTest();

                sr.removeProduct(p);

                Product returnP = DBProduct.getInstance().getProductByID(p.getProductID());
                Assert.AreEqual(returnP, null);
                Assert.AreEqual(store.getProductList().Contains(p), false);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
}

        [TestMethod()]
        public void removeProductFailTest()
        {
            try
            {
                addProductSuccTest();
                sr.removeProduct(p1);

                Assert.Fail();
            }
            catch (StoreException) {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void setProductPriceSuccTest()
        {
            try
            {
                addProductSuccTest();

                sr.setProductPrice(p, 5);
                Assert.AreEqual(5, p.getPrice());
            }
            catch (Exception) {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void setProductNameSuccTest()
        {
            try
            {
                addProductSuccTest();
                sr.setProductName(p, "other name");

                Assert.AreEqual("other name", p.getProductName());
            }
            catch (Exception) {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void addToProductQuantitySuccTest()
        {
            try
            {
                addProductSuccTest();
                sr.addToProductQuantity(p, 5);

                Assert.AreEqual(15, p.getQuantityLeft());
            }
            catch (Exception){
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void decFromProductQuantitySuccTest()
        {
            try
            {
                addProductSuccTest();
                sr.decFromProductQuantity(p, 10);

                Assert.AreEqual(0, p.getQuantityLeft());
            }
            catch (Exception)
            {
                Assert.Fail();
            }
}

        [TestMethod()]
        public void addManagerSuccTest()
        {
            try
            {
                sr.addManager(session2.getSubscribedUser(), per);
                Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()) is StoreManager, true);
                Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()).getAppointedBy(),
                    session1.getSubscribedUser());
                Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()),
                    session2.getSubscribedUser().getStoreRole(store));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void addManagerFailTest()
        {
            try
            {
                sr.addManager(session1.getSubscribedUser(), per);
                Assert.AreEqual(0, session2.getSubscribedUser().getStoreRoles().Count);
                Assert.AreEqual(true, store.getStoreRole(session1.getSubscribedUser()) is StoreOwner);
            }
            catch (RoleException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void removeManagerSuccTest()
        {
            try
            {
                sr.addManager(session3.getSubscribedUser(), per);
                sr.remove(session3.getSubscribedUser());
                Assert.AreEqual(store.getStoreRole(session3.getSubscribedUser()), null);
                Assert.AreEqual(session3.getSubscribedUser().getStoreRoles().Count, 0);
            }
            catch (RoleException)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void removeManagerFailTest()
        {
            try
            {
                sr.remove(session1.getSubscribedUser());
                sr.addManager(session2.getSubscribedUser(), per);
                sr.remove(session3.getSubscribedUser());
                Assert.Fail();
            }
            catch (RoleException)
            {
            }
        }

        [TestMethod()]
        public void addOwnerSuccTest()
        {
            try
            {
                sr.addOwner(session2.getSubscribedUser());
                Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()) is StoreOwner, true);
                Assert.AreEqual(store.getNumberOfOwners(), 2);
                Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()).getAppointedBy(),
                    session1.getSubscribedUser());
                Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()),
                    session2.getSubscribedUser().getStoreRole(store));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void addOwnerFailTest()
        {
            try
            {
                sr.addOwner(session1.getSubscribedUser());
                Assert.AreEqual(0, session2.getSubscribedUser().getStoreRoles().Count);

                sr.addOwner(session2.getSubscribedUser());
                sr.addOwner(session2.getSubscribedUser());

                sr.addManager(session3.getSubscribedUser(), per);
                sr.addOwner(session3.getSubscribedUser());
                Assert.Fail();
            }
            catch (RoleException)
            {
            }
        }

    }
}
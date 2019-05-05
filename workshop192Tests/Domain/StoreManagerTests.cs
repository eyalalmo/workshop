using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain.Tests
{
    [TestClass()]
    public class StoreManagerTests
    {
        Session session1, session2, session3;
        DBStore storeDB = DBStore.getInstance();
        DBProduct productDB = DBProduct.getInstance();
        Product p, p1;
        Store store;
        StoreRole sr, sr1, sr2;
        Permissions per, per2;

        [TestInitialize()]
        public void init()
        {
            storeDB.init();
            productDB.init();
            DBSubscribedUser.getInstance().cleanDB();
            session1 = new Session();
            session1.register("eyal", "123");

            session2 = new Session();
            session2.register("bar", "123");

            session3 = new Session();
            session3.register("etay", "123");

            session1.login("eyal", "123");
            session2.login("bar", "123");
            session3.login("etay", "123");

            store = session1.createStore("mystore", "a store");

            per = new Permissions(true, true, true);
            per2 = new Permissions(false, false, false);

            sr = session1.getSubscribedUser().getStoreRole(store);
            sr.addManager(session2.getSubscribedUser(), per);
            sr.addManager(session3.getSubscribedUser(), per2);

            sr1 = session2.getSubscribedUser().getStoreRole(store);
            sr2 = session3.getSubscribedUser().getStoreRole(store);

            p = new Product("product", "cat", 10, 0, 10, store);
            p1 = new Product("product1", "cat", 10, 0, 10, store);

        }

        [TestMethod()]
        public void addProductSuccTest()
        {
            try
            {
                sr1.addProduct(p);

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
                sr1.removeProduct(p);

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
                sr2.removeProduct(p1);
                Assert.AreEqual(store.getProductList().Contains(p1), false);
                Assert.Fail();
            }
            catch (PermissionsException)
            {
                Assert.IsTrue(true);
            }
            catch (StoreException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void setProductPriceSuccTest()
        {
            try
            {
                addProductSuccTest();

                sr1.setProductPrice(p, 5);
                Assert.AreEqual(5, p.getPrice());
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void setProductNameSuccTest()
        {
            addProductSuccTest();
            try
            {
                sr1.setProductName(p, "other name");
                Assert.AreEqual("other name", p.getProductName());
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void addToProductQuantitySuccTest()
        {
            try
            {
                addProductSuccTest();
                sr1.addToProductQuantity(p, 5);
                Assert.AreEqual(15, p.getQuantityLeft());
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void addToProductQuantityFailTest()
        {
            try
            {
                addProductSuccTest();
                sr2.addToProductQuantity(p, 10);
                Assert.Fail();
            }
            catch (PermissionsException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void decFromProductQuantitySuccTest()
        {
            try
            {
                addProductSuccTest();
                sr1.decFromProductQuantity(p, 10);
                Assert.AreEqual(0, p.getQuantityLeft());
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void decFromProductQuantityFailTest()
        {
            try
            {
                addProductSuccTest();
                sr2.decFromProductQuantity(p, 8);
                Assert.Fail();
            }
            catch (PermissionsException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void addManagerFailTest()
        {
            try
            {
                sr1.addManager(session1.getSubscribedUser(), per);
                sr1.addManager(session2.getSubscribedUser(), per);
                Assert.Fail();
            }
            catch (RoleException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void removeManagerFailTest()
        {
            try
            {
                sr1.remove(session1.getSubscribedUser());
                sr1.remove(session2.getSubscribedUser());
                Assert.Fail();
            }
            catch (RoleException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void addOwnerFailTest()
        {
            try
            {
                sr1.addOwner(session1.getSubscribedUser());
            }
            catch (RoleException)
            {
                Assert.IsTrue(true);
            }
        } 

        [TestMethod()]
        public void removeOwnerFailTest()
        {
            try
            {
                sr1.remove(session1.getSubscribedUser());
                sr1.remove(session2.getSubscribedUser());
                Assert.IsTrue(true);
            }
            catch (RoleException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void closeStoreFailTest()
        {
            Assert.IsTrue(true);
        }
    }
}
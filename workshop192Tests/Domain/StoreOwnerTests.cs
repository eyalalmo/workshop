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
    public class StoreOwnerTests
    {
        Session session1, session2, session3;
        DBStore storeDB = DBStore.getInstance();
        DBProduct productDB = DBProduct.getInstance();
        Product p, p1;
        Store store;
        StoreRole sr, sr1, sr2;
        Permissions per;

        [TestInitialize()]
        public void init()
        {
            storeDB.init();
            productDB.init();
            session1 = new Session();
            session1.register("eyal", "123");

            session2 = new Session();
            session2.register("bar", "123");

            session3 = new Session();
            session3.register("etay", "123");

            session1.login("eyal", "123");
            store = session1.createStore("mystore", "a store");
            sr = session1.getSubscribedUser().getStoreRole(store);
            p = new Product("product", "cat", 10, 0, 10, store);
            p1 = new Product("product1", "cat", 10, 0, 10, store);

            per = new Permissions(true, true, true);
        }

        [TestMethod()]
        public void addProductSuccTest()
        {         
            Assert.AreEqual("", sr.addProduct(p));

            Product returnP = DBProduct.getInstance().getProductByID(p.getProductID());

            Assert.AreEqual(returnP, p);
            Assert.AreEqual(store.getProductList().Contains(p), true);
        }

        [TestMethod()]
        public void addProductFailTest()
        {
            Assert.AreNotEqual("", sr.addProduct(p));
            
            p = new Product("product", "cat", 5, 6, 10, store);
            Assert.AreNotEqual("", sr.addProduct(p));

            p = new Product("product", "cat", 5, 5, -6, store);
            Assert.AreNotEqual("", sr.addProduct(p));

            p = new Product("product", "cat", 5, 6, 10, null);
            Assert.AreNotEqual("", sr.addProduct(p));
            Assert.AreEqual(store.getProductList().Contains(p), false);
        }
        
        [TestMethod()]
        public void removeProductSuccTest()
        {
            sr.addProduct(p);
            Assert.AreEqual("", sr.removeProduct(p));

            Product returnP = DBProduct.getInstance().getProductByID(p.getProductID());
            Assert.AreEqual(returnP, null);
            Assert.AreEqual(store.getProductList().Contains(p), false);
        }

        [TestMethod()]
        public void removeProductFailTest()
        {
            sr.addProduct(p);

            Assert.AreNotEqual("", sr.removeProduct(p1));
            Assert.AreEqual(store.getProductList().Contains(p1), false);
        }

        [TestMethod()]
        public void setProductPriceSuccTest()
        {
            sr.addProduct(p);

            Assert.AreEqual("", sr.setProductPrice(p, 5));
            Assert.AreEqual(5, p.getPrice());
        }

        [TestMethod()]
        public void setProductPriceFailTest()
        {
            sr.addProduct(p);

            Assert.AreNotEqual("", sr.setProductPrice(p, -5));
            Assert.AreNotEqual(-5, p.getPrice());
        }

        [TestMethod()]
        public void setProductNameSuccTest()
        {
            sr.addProduct(p);

            Assert.AreEqual("", sr.setProductName(p, "other name"));
            Assert.AreEqual("other name", p.getProductName());
        }

        [TestMethod()]
        public void setProductNameFailTest()
        {
            sr.addProduct(p);

            Assert.AreNotEqual("", sr.setProductName(p, null));
            Assert.AreNotEqual(null, p.getProductName());
        }

        [TestMethod()]
        public void addToProductQuantitySuccTest()
        {
            sr.addProduct(p);

            Assert.AreEqual("", sr.addToProductQuantity(p, 5));
            Assert.AreEqual(15, p.getQuantityLeft());
        }

        [TestMethod()]
        public void addToProductQuantityFailTest()
        {
            sr.addProduct(p);

            Assert.AreNotEqual("", sr.addToProductQuantity(p, -12));
            Assert.AreNotEqual(-2, p.getQuantityLeft());
        }

        [TestMethod()]
        public void decFromProductQuantitySuccTest()
        {
            sr.addProduct(p);

            Assert.AreEqual("", sr.decFromProductQuantity(p, 10));
            Assert.AreEqual(20, p.getQuantityLeft());
        }

        [TestMethod()]
        public void decFromProductQuantityFailTest()
        {
            sr.addProduct(p);

            Assert.AreNotEqual("", sr.decFromProductQuantity(p, 12));
            Assert.AreNotEqual(-2, p.getQuantityLeft());
            Assert.AreNotEqual("", sr.decFromProductQuantity(p, -12));
            Assert.AreNotEqual(22, p.getQuantityLeft());
        }

        [TestMethod()]
        public void addManagerSuccTest()
        {          
            Assert.AreEqual("", sr.addManager(session2.getSubscribedUser(), per));
            Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()) is StoreManager, true);
            Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()).getAppointedBy(), 
                session1.getSubscribedUser());
            Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()), 
                session2.getSubscribedUser().getStoreRole(store));
        }

        [TestMethod()]
        public void addManagerFailTest()
        {
            Assert.AreNotEqual("", sr.addManager(session1.getSubscribedUser(), per));
            Assert.AreEqual(0, session2.getSubscribedUser().getStoreRoles().Count);
            Assert.AreEqual(true, store.getStoreRole(session1.getSubscribedUser()) is StoreOwner);

            Assert.AreNotEqual("", sr.addManager(session2.getSubscribedUser(), null));
            Assert.AreEqual(0, session2.getSubscribedUser().getStoreRoles().Count);
            Assert.AreEqual(null, store.getStoreRole(session2.getSubscribedUser()));
        }

        [TestMethod()]
        public void removeManagerSuccTest()
        {
            sr.addManager(session2.getSubscribedUser(), per);
            Assert.AreEqual(sr.remove(session2.getSubscribedUser()), "");
            Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()), null);
            Assert.AreEqual(session2.getSubscribedUser().getStoreRoles().Count, 0);
        }

        [TestMethod()]
        public void removeManagerFailTest()
        {
            Assert.AreNotEqual(sr.remove(session1.getSubscribedUser()), "");

            sr.addManager(session2.getSubscribedUser(), per);

            Assert.AreNotEqual(sr.remove(session3.getSubscribedUser()), "");
        }

        [TestMethod()]
        public void addOwnerSuccTest()
        {
            Assert.AreEqual("", sr.addOwner(session2.getSubscribedUser()));
            Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()) is StoreOwner, true);
            Assert.AreEqual(store.getNumberOfOwners(), 2);
            Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()).getAppointedBy(),
                session1.getSubscribedUser());
            Assert.AreEqual(store.getStoreRole(session2.getSubscribedUser()),
                session2.getSubscribedUser().getStoreRole(store));
        }

        [TestMethod()]
        public void addOwnerFailTest()
        {
            Assert.AreNotEqual("", sr.addOwner(session1.getSubscribedUser()));
            Assert.AreEqual(0, session2.getSubscribedUser().getStoreRoles().Count);

            sr.addOwner(session2.getSubscribedUser());
            Assert.AreNotEqual("", sr.addOwner(session2.getSubscribedUser()));

            sr.addManager(session3.getSubscribedUser(), per);
            Assert.AreNotEqual("", sr.addOwner(session3.getSubscribedUser()));
        }

        [TestMethod()]
        public void removeOwnerSuccTest()
        {
            sr.addOwner(session2.getSubscribedUser());

            Assert.AreEqual(sr.remove(session2.getSubscribedUser()), "");
            Assert.AreEqual(session2.getSubscribedUser().getStoreRoles().Count, 0);
            Assert.AreEqual(store.getNumberOfOwners(), 1);
        }

        [TestMethod()]
        public void removeOwnerFailTest()
        {
            sr.addOwner(session2.getSubscribedUser());
            Assert.AreNotEqual(sr.remove(session3.getSubscribedUser()), "");
            Assert.AreNotEqual(sr.remove(session1.getSubscribedUser()), "");
        }

        [TestMethod()]
        public void closeStoreSuccTest()
        {
            Assert.Fail();
        }
    }
}
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
        DBStore storeDB;
        DBProduct productDB;
        Product p, p1;
        Store store;
        StoreRole sr, sr1, sr2;
        Permissions per, per2;

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
            session2.login("bar", "123");
            session2.login("etay", "123");

            store = session1.createStore("mystore", "a store", session1.getSubscribedUser());

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
            Assert.AreEqual("", sr2.addProduct(p));

            Product returnP = DBProduct.getInstance().getProductByID(p.getProductID());

            Assert.AreEqual(returnP, p);
            Assert.AreEqual(store.getProductList().Contains(p), true);
        }

        [TestMethod()]
        public void addProductFailTest()
        {
            Assert.AreNotEqual("", sr2.addProduct(p));
            
            p = new Product("product", "cat", 5, 6, 10, store);
            Assert.AreNotEqual("", sr1.addProduct(p));

            p = new Product("product", "cat", 5, 5, -6, store);
            Assert.AreNotEqual("", sr1.addProduct(p));

            p = new Product("product", "cat", 5, 6, 10, null);
            Assert.AreNotEqual("", sr1.addProduct(p));
            Assert.AreEqual(store.getProductList().Contains(p), false);
        }
        
        [TestMethod()]
        public void removeProductSuccTest()
        {
            sr.addProduct(p);
            Assert.AreEqual("", sr1.removeProduct(p));

            Product returnP = DBProduct.getInstance().getProductByID(p.getProductID());
            Assert.AreEqual(returnP, null);
            Assert.AreEqual(store.getProductList().Contains(p), false);
        }

        [TestMethod()]
        public void removeProductFailTest()
        {
            sr.addProduct(p);

            Assert.AreNotEqual("", sr2.removeProduct(p1));
            Assert.AreEqual(store.getProductList().Contains(p1), false);
        }

        [TestMethod()]
        public void setProductPriceSuccTest()
        {
            sr.addProduct(p);

            Assert.AreEqual("", sr1.setProductPrice(p, 5));
            Assert.AreEqual(5, p.getPrice());
        }

        [TestMethod()]
        public void setProductPriceFailTest()
        {
            sr.addProduct(p);

            Assert.AreNotEqual("", sr2.setProductPrice(p, 10));
            Assert.AreNotEqual("", sr1.setProductPrice(p, -5));
            Assert.AreNotEqual(-5, p.getPrice());
        }

        [TestMethod()]
        public void setProductNameSuccTest()
        {
            sr.addProduct(p);

            Assert.AreEqual("", sr1.setProductName(p, "other name"));
            Assert.AreEqual("other name", p.getProductName());
        }

        [TestMethod()]
        public void setProductNameFailTest()
        {
            sr.addProduct(p);

            Assert.AreNotEqual("", sr2.setProductName(p, "hi"));
            Assert.AreNotEqual("", sr1.setProductName(p, null));
            Assert.AreNotEqual(null, p.getProductName());
        }

        [TestMethod()]
        public void addToProductQuantitySuccTest()
        {
            sr.addProduct(p);

            Assert.AreEqual("", sr1.addToProductQuantity(p, 5));
            Assert.AreEqual(15, p.getQuantityLeft());
        }

        [TestMethod()]
        public void addToProductQuantityFailTest()
        {
            sr.addProduct(p);

            Assert.AreNotEqual("", sr2.addToProductQuantity(p, 10));
            Assert.AreNotEqual("", sr1.addToProductQuantity(p, -12));
            Assert.AreNotEqual(-2, p.getQuantityLeft());
        }

        [TestMethod()]
        public void decFromProductQuantitySuccTest()
        {
            sr.addProduct(p);

            Assert.AreEqual("", sr1.decFromProductQuantity(p, 10));
            Assert.AreEqual(20, p.getQuantityLeft());
        }

        [TestMethod()]
        public void decFromProductQuantityFailTest()
        {
            sr.addProduct(p);

            Assert.AreNotEqual("", sr2.decFromProductQuantity(p, 8));
            Assert.AreNotEqual("", sr.decFromProductQuantity(p, 12));
            Assert.AreNotEqual(-2, p.getQuantityLeft());
            Assert.AreNotEqual("", sr.decFromProductQuantity(p, -12));
            Assert.AreNotEqual(22, p.getQuantityLeft());
        }

        [TestMethod()]
        public void addManagerFailTest()
        {
            Assert.AreNotEqual("", sr1.addManager(session1.getSubscribedUser(), per));
            Assert.AreNotEqual("", sr1.addManager(session2.getSubscribedUser(), per));
        }

        [TestMethod()]
        public void removeManagerFailTest()
        {
            Assert.AreNotEqual(sr1.remove(session1.getSubscribedUser()), "");
            Assert.AreNotEqual(sr1.remove(session2.getSubscribedUser()), "");
        }
        
        [TestMethod()]
        public void addOwnerFailTest()
        {
            Assert.AreNotEqual("", sr1.addOwner(session1.getSubscribedUser()));
        }

        [TestMethod()]
        public void removeOwnerFailTest()
        {
            Assert.AreNotEqual(sr1.remove(session1.getSubscribedUser()), "");
            Assert.AreNotEqual(sr1.remove(session2.getSubscribedUser()), "");
        }

        [TestMethod()]
        public void closeStoreFailTest()
        {
            Assert.Fail();
        }
    }
}
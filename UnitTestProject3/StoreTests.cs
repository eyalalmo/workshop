using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;

namespace UnitTestProject3
{
    [TestClass]
    public class StoreTests
    {
        private Store store;

        [TestInitialize()]
        public void initial()
        {
            store = new Store("Makolet", "groceryStore");
        }
        [TestMethod]
        public void testAddRemoveStore()
        {
            Product p1 = new Product("p1", "ff", 56, 2, 10, store.getStoreID());
            Product p2 = new Product("p2", "ff", 56, 2, 10, store.getStoreID());
            Product p3 = new Product("p3", "ff", 56, 2, 10, store.getStoreID());
            store.addProduct(p1);
            store.addProduct(p2);
            store.addProduct(p3);

            Assert.AreEqual(true, store.productExists(p1));
            Assert.AreEqual(true, store.productExists(p1.getProductID()));
            Assert.AreEqual(true, store.productExists(p3));
            store.removeProduct(p3);
            Assert.AreEqual(false, store.productExists(p3));
        }

    }
}

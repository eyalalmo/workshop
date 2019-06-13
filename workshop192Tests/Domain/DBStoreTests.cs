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
    public class DBStoreTests
    {
        Store s;
        SubscribedUser su;
        StoreRole sr;
       [TestInitialize()]
        public void TestInitialize()
        {
            MarketSystem.initTestWitOutRead();
            s = new Store("store", "store");
            DBStore.getInstance().addStore(s);
            su = new SubscribedUser("u", "u", new ShoppingBasket());
            sr = new StoreOwner(null, su, s);

        }

        [TestMethod()]
        public void removeStoreRoleTest()
        {
            DBStore.getInstance().addStoreRole(sr);
            DBStore.getInstance().removeStoreRole(sr);
            Assert.AreEqual(DBStore.getInstance().getStoreRole(s,su), null);
        }


        [TestMethod()]
        public void addStoreRoleTest()
        {
            DBStore.getInstance().addStoreRole(sr);
            Assert.AreEqual(DBStore.getInstance().getStoreRole(s, su),sr);

        }

        [TestMethod()]
        public void addStoreTest()
        {
            Store s1 = new Store("store", "store");
            int ID = DBStore.getInstance().addStore(s1);
            Assert.AreNotEqual(DBStore.getInstance().getStore(ID), null);
        }

    
    }
}
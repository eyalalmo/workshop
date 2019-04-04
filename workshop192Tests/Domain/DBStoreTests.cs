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
            DBStore.getInstance().DBinit();
            s = new Store("store", "store");
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
            int ID = DBStore.getInstance().addStore(s);
            Assert.AreNotEqual(DBStore.getInstance().getStore(ID), null);
        }

        [TestMethod()]
        public void removeStoreTest()
        {
            int ID = DBStore.getInstance().addStore(s);
            Assert.AreEqual(DBStore.getInstance().removeStore(s),"");
            Assert.AreEqual(DBStore.getInstance().getStore(ID), null);
            Assert.AreNotEqual(DBStore.getInstance().removeStore(s), "");
        }

        [TestMethod()]
        public void closeStoreTest()
        {
            int ID = DBStore.getInstance().addStore(s);
            DBStore.getInstance().closeStore(s);
            Assert.IsFalse(s.getStatus());//
        }


    }
}
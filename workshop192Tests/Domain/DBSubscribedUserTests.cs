using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Bridge;

namespace workshop192.Domain.Tests
{
    [TestClass()]
    public class DBSubscribedUserTests
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            MarketSystem.initTestWitOutRead();

        }
        [TestMethod()]
        public void logoutTest()
        {

            DBSubscribedUser db = DBSubscribedUser.getInstance();
            Session s = new Session();
            string pass = DomainBridge.getInstance().encryptPassword("etay123");
            s.register("etay", pass);
            s.login("etay", "etay123");
            if (db.getSubscribedUser("etay") == null)
                Assert.Fail();
            s.logout();
            Assert.AreEqual(null, db.getloggedInUser("etay"));

        }

        [TestMethod()]
        public void registerTest()
        {
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            Session s = new Session();
            string pass = DomainBridge.getInstance().encryptPassword("etay123");
            s.register("etay123", pass);
            Assert.AreNotEqual(db.getSubscribedUser("etay"), null);            
        }

        [TestMethod()]
        public void getSubscribedUserTest()
        {
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            string pass = DomainBridge.getInstance().encryptPassword("etay123");
            Session s = new Session();
            s.register("etay11", pass);
            Assert.AreNotEqual(db.getSubscribedUser("etay"), null);
        }

        [TestMethod()]
        public void loginTest()
        {
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            Session s = new Session();
            string pass = DomainBridge.getInstance().encryptPassword("etay123");
            s.register("etay", pass);
            s.login("etay", "etay123");
            Assert.AreEqual(db.getloggedInUser("etay"), s.getSubscribedUser());

        }
    }
}

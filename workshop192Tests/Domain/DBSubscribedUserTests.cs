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
    public class DBSubscribedUserTests
    {

        [TestMethod()]
        public void logoutTest()
        {
            
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            db.init();
            Session s = new Session();

            s.register("etay", "etay");
            s.login("etay", "etay");
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
            s.register("etay123", "etay123");
            Assert.AreNotEqual(db.getSubscribedUser("etay"), null);            
        }

        [TestMethod()]
        public void getSubscribedUserTest()
        {
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            Session s = new Session();
            s.register("etay11", "etay11");
            Assert.AreNotEqual(db.getSubscribedUser("etay"), null);
            db.init();
        }

        [TestMethod()]
        public void loginTest()
        {
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            db.init();
            Session s = new Session();
            s.register("etay", "etay");
            s.login("etay", "etay");
            Assert.AreEqual(db.getloggedInUser("etay"), s.getSubscribedUser());
            db.init();
        }
    }
}

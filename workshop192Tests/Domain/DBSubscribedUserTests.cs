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
        public void DBSubscribedUserTest()
        {


        }

        [TestMethod()]
        public void getInstanceTest()
        {




        }

        [TestMethod()]
        public void logoutTest()
        {
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            Session s = new Session();

            s.register("etay", "etay");
            s.login("etay", "etay");
            if (db.getSubscribedUser("etay") == null)
                Assert.Fail();
            s.logout();
            Assert.AreEqual("", db.getloggedInUser("etay"));

        }

        [TestMethod()]
        public void registerTest()
        {
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            Session s = new Session();
            s.register("etay", "etay");
            Assert.AreNotEqual(db.getSubscribedUser("etay"), null);
            db.cleanDB();
        }

        [TestMethod()]
        public void getSubscribedUserTest()
        {
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            Session s = new Session();
            s.register("etay", "etay");
            Assert.AreNotEqual(db.getSubscribedUser("etay"), null);
            db.cleanDB();
        }

        [TestMethod()]
        public void loginTest()
        {
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            Session s = new Session();
            s.register("etay", "etay");
            s.login("etay", "etay");
            Assert.AreEqual(db.getloggedInUser("etay"), "");
            db.cleanDB();
        }

        [TestMethod()]
        public void removeTest()
        {
            ////////////
        }

        [TestMethod()]
        public void initTest()
        {
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            Session s = new Session();
            s.register("etay", "etay");
            db.cleanDB();
            Assert.AreEqual(db.getSubscribedUser("etay"), null);
            db.cleanDB();
        }
    }
}

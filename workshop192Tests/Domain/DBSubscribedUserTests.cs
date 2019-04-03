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
            if(db.getSubscribedUser("etay")==null)
                Assert.Fail();
            s.logout();
            Assert.AreEqual(null, db.getloggedInUser("etay"));
          
        }

        [TestMethod()]
        public void registerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void getSubscribedUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void loginTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void removeTest()
        {
            Assert.Fail();
        }
    }
}
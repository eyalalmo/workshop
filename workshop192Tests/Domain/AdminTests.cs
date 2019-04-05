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
    public class AdminTests
    {
        private Session session = new Session();

        [TestMethod()]
        public void createStoreTest()
        {
            UserState state = new Admin();
            Assert.IsNull(state.createStore("ToyRUs", "lots of toys", null));
        }

        [TestMethod()]
        public void getPurchaseHistoryTest()
        {
            UserState state = new Admin();
            Assert.IsTrue(Equals(state.getPurchaseHistory(null), "ERROR: No purchase history in Admin"));
        }

        [TestMethod()]
        public void loginTest()
        {
            Admin admin = new Admin();
            Assert.IsTrue(Equals(admin.login("admin", "1234", session), "ERROR: User already logged in"));
        }

        [TestMethod()]
        public void logoutTest()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            session.login("admin", "1234");
            UserState state = session.getState();
            Assert.IsTrue(state is Admin);
            Assert.IsTrue(Equals(state.logout(session.getSubscribedUser(), session), ""));
            Assert.IsTrue(session.getState() is Guest);
        }

        [TestMethod()]
        public void registerTest()
        {
            UserState state = new Admin();
            Assert.IsTrue(Equals(state.register("shalom", "1111", null), "ERROR: User already registered"));
        }
    }
}

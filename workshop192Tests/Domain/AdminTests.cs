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
            try
            {
                Admin admin = new Admin();
                admin.login("admin", "1234", session);
                Assert.Fail();
            }
            catch (LoginException le) {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void logoutTest()
        {
            try
            {
                DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
                session.login("admin", "1234");
                UserState state = session.getState();
                Assert.IsTrue(state is Admin);
                state.logout(session.getSubscribedUser(), session);
                Assert.IsTrue(session.getState() is Guest);
            }
            catch (Exception e) {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void registerTest()
        {
            try
            {
                UserState state = new Admin();
                state.register("shalom", "1111", null);
                Assert.IsTrue(true);
            }
            catch (LoginException le)
            {
                Assert.Fail();
            }
        }
    }
}

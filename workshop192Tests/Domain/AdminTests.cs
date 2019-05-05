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
            Session session = new Session();
            UserState state = new LoggedIn();
            try
            {
                state.login("david", "david", session);
            }
            catch (LoginException e)
            {
                Assert.IsTrue(true);
            }
            Assert.Fail();
        }

        [TestMethod()]
        public void logoutTest()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            session.login("admin", "1234");
            UserState state = session.getState();
            Assert.IsTrue(state is Admin);
            state.logout(session.getSubscribedUser(), session);
            Assert.IsTrue(session.getState() is Guest);
        }

        [TestMethod()]
        public void registerTest()
        {
            Session session = new Session();
            UserState state = new LoggedIn();
            try
            {
                state.register("ben", "bat", session);
            }
            catch (RegisterException e)
            {
                Assert.IsTrue(true);
            }
            Assert.Fail();
        }
    }

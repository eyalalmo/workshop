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
            state.createStore("ToyRUs", "lots of toys", new SubscribedUser("aa", "aa", null));
        }

        [TestMethod()]
        public void getPurchaseHistoryTest()
        {
            try
            {
                UserState state = new Admin();
                state.getPurchaseHistory(null);
                Assert.Fail();
            }
            catch (Exception) {
                Assert.IsTrue(true);
            }
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
            catch (LoginException)
            {
                Assert.IsTrue(true);
            }

            try
            {
                DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
                session.login("admin", "1234");
                UserState state = session.getState();
                Assert.IsTrue(state is Admin);
                state.logout(session.getSubscribedUser(), session);
                Assert.IsTrue(session.getState() is Guest);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                UserState state = new Admin();
                state.register("shalom", "1111", null);
                Assert.Fail();
            }
            catch (RegisterException)
            {
                Assert.IsTrue(true);
            }
        }
    }
}
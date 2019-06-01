
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
    public class LoggedInTests
    {
        private Session session;
        private UserState state;

        public void LoggedInTest()
        {
        }
        [TestInitialize()]
        public void init()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            dbsubscribedUser.initTests();
            DBStore.getInstance().cleanDB();
        }

        [TestMethod()]
        public void closeStoreTest()
        {
            UserState state = new LoggedIn();
            try
            {
                state.closeStore(null);
                Assert.Fail();
            }
            catch (UserStateException)
            {
                Assert.IsTrue(true);
            }
            
        }

        [TestMethod()]
        public void createStoreTest()
        {
            Session session = new Session();
            session.register("yael", "yael");
            session.login("yael", "yael");
            Store store = session.getState().createStore("Wallmart", "sells everything", session.getSubscribedUser());
            Assert.AreNotEqual(store,null);
        }

        [TestMethod()]
        public void loginTest()
        {
            UserState state = new Guest();
            try
            {
                registerTest();
                state.login("ben", "bat", session);
            }
            catch(LoginException)
            {
                Assert.Fail();
            }
            
        }

        [TestMethod()]
        public void logoutTest()
        {
            try
            {
                loginTest();
                session.getState().logout(session.getSubscribedUser(), session);
            }
            catch (LoginException)
            {
                Assert.Fail();
            }

        }

        [TestMethod()]
        public void registerTest()
        {
            session = new Session();
            state = new Guest();
            try
            {
                state.register("ben", "bat", session);
            }
            catch(RegisterException)
            {
                Assert.Fail();
            }
            
        }

        [TestMethod()]
        public void removeUserTest()
        {
            state = new LoggedIn();
            try
            {
                state.removeUser("ben");
                Assert.Fail();
            }
            catch (UserStateException)
            {
                Assert.IsTrue(true);
            }
            
        }
    }
}


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
        public void LoggedInTest()
        {
        }
        [TestInitialize()]
        public void init()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            dbsubscribedUser.init();
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
            Session session = new Session();
            UserState state = new LoggedIn();
            try
            {
                state.login("david", "david", session);
                Assert.Fail();
            }
            catch(LoginException)
            {
                Assert.IsTrue(true);
            }
            
        }

        [TestMethod()]
        public void logoutTest()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            SubscribedUser sub2 = new SubscribedUser("Gal", "Gadot", new ShoppingBasket());
            dbsubscribedUser.register(sub2);

            Session session = new Session();
            UserState state = session.getState();
            state.login("Gal", "Gadot", session);
            SubscribedUser user = session.getSubscribedUser();
            session.getState().logout(user, session);
            Assert.IsTrue(session.getState() is Guest);
            Assert.IsNull(dbsubscribedUser.getloggedInUser("Gal"));
        }

        [TestMethod()]
        public void registerTest()
        {
            Session session = new Session();
            UserState state = new LoggedIn();
            try
            {
                state.register("ben", "bat", session);
                Assert.Fail();
            }
            catch(RegisterException)
            {
                Assert.IsTrue(true);
            }
            
        }

        [TestMethod()]
        public void removeUserTest()
        {
            UserState state = new LoggedIn();
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


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
    public class LoggedInTests
    {
        private Session session;
        private UserState state;

        [TestInitialize()]
        public void init()
        {
            MarketSystem.initTestWitOutRead();
            //DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            //dbsubscribedUser.initTests();
            //DBStore.getInstance().cleanDB();
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
            DBSubscribedUser db = DBSubscribedUser.getInstance();
            Session s = new Session();
            string pass = DomainBridge.getInstance().encryptPassword("yael");
            s.register("yael", pass);
            s.login("yael", "yael");
            Store store = s.getState().createStore("Wallmart", "sells everything", s.getSubscribedUser());
            Assert.AreNotEqual(store,null);
        }


        [TestMethod()]
        public void logoutTest()
        {
            try
            {
                DBSubscribedUser db = DBSubscribedUser.getInstance();
                Session s = new Session();
                string pass = DomainBridge.getInstance().encryptPassword("etay123");
                s.register("etay", pass);
                s.login("etay", "etay123");
                s.getState().logout(s.getSubscribedUser(),s);
                Assert.IsTrue(s.getState() is Guest);
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

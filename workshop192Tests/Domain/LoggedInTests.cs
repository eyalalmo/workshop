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

        [TestMethod()]
        public void closeStoreTest()
        {
            UserState state = new LoggedIn();
            String res = state.closeStore(null);
            Assert.IsTrue(Equals("ERROR: not an admin", res));
        }

        [TestMethod()]
        public void createStoreTest()
        {
            Session session = new Session();
            session.register("yael", "yael");
            session.login("yael", "yael");
            Store store = session.getState().createStore("Wallmart", "sells everything", session.getSubscribedUser());
            Assert.IsTrue(store != null);
            DBStore.getInstance().cleanDB();
        }

        [TestMethod()]
        public void getPurchaseHistoryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void loginTest()
        {
            Session session = new Session();
            UserState state = new LoggedIn();
            Assert.IsTrue(Equals("ERROR: User already logged in", state.login("david", "david", session)));
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
            dbsubscribedUser.cleanDB();
        }

        [TestMethod()]
        public void registerTest()
        {
            Session session = new Session();
            UserState state = new LoggedIn();
            Assert.IsTrue(Equals("ERROR: User already registered", state.register("ben", "bat", session)));
        }

        [TestMethod()]
        public void removeUserTest()
        {
            UserState state = new LoggedIn();
            Assert.IsTrue(Equals("ERROR: not an admin", state.removeUser("benny")));

        }
    }
}

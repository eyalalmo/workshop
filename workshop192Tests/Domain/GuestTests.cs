
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
    public class GuestTests
    {

        public void GuestTest()
        {
        }

        [TestMethod()]
        public void closeStoreTest()
        {
            UserState state = new Guest();
            String res = state.closeStore(null);
            Assert.IsTrue(Equals("ERROR: not an admin", res));
        }

        [TestMethod()]
        public void createStoreTest()
        {
            UserState state = new Guest();
            SubscribedUser sub = new SubscribedUser("dilan", "aaa", new ShoppingBasket());
            Assert.IsNull(state.createStore("Wallmart", "sells everything", sub));
        }

        [TestMethod()]
        public void getPurchaseHistoryTest()
        {
            UserState state = new Guest();
            SubscribedUser sub = new SubscribedUser("Benny", "BENNY", new ShoppingBasket());
            Assert.IsTrue(Equals("ERROR: not a subscribed user", state.getPurchaseHistory(sub)));
        }

        [TestMethod()]
        public void loginTest()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            SubscribedUser sub1 = new SubscribedUser("Danny", "Shovevani", new ShoppingBasket());
            dbsubscribedUser.register(sub1);

            Session session = new Session();
            UserState state = session.getState();
            Assert.IsTrue(Equals(state.login("bob", "dilan", session), "ERROR: username does not exist"));
            Assert.IsTrue(Equals(state.login("Danny", "Shovevani", session), ""));
            Assert.IsTrue(session.getState() is LoggedIn);

            Session session2 = new Session();
            UserState state2 = session2.getState();
            Assert.IsTrue(Equals(state.login("Danny", "aaaa", session2), "ERROR: password incorrect"));
            dbsubscribedUser.cleanDB();
        }

        [TestMethod()]
        public void logoutTest()
        {
            UserState state = new Guest();
            Assert.IsTrue(Equals(state.logout(null, null), "ERROR: not logged in"));

        }

        [TestMethod()]
        public void registerTest()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            SubscribedUser sub3 = new SubscribedUser("Benny", "aaaa", new ShoppingBasket());
            dbsubscribedUser.register(sub3);

            Session session = new Session();
            UserState state = session.getState();
            Assert.IsTrue(Equals(state.register("Benny", "aaaa", session), "ERROR: username already exists"));
            Assert.IsTrue(Equals(state.register("Viva", "Diva", session), ""));
            Assert.IsTrue(state is Guest);
            dbsubscribedUser.cleanDB();
        }

        [TestMethod()]
        public void removeUserTest()
        {
            Session session = new Session();
            UserState state = session.getState();
            Assert.IsTrue(Equals(state.removeUser("abcd"), "ERROR: not an admin"));

        }
    }
}

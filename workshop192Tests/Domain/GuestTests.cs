
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
        [TestInitialize()]
        public void init()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            dbsubscribedUser.cleanDB();
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
            catch (UserStateException e)
            {
                Assert.IsTrue(true);
            }
            
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
        public void loginTest1()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            SubscribedUser sub1 = new SubscribedUser("Danny", dbsubscribedUser.encryptPassword("Shovevani"), new ShoppingBasket());
            dbsubscribedUser.register(sub1);

            Session session = new Session();
            UserState state = session.getState();
            try
            {
                state.login("bob", "dilan", session);
                Assert.Fail();
            } catch (LoginException e)
            {
                Assert.IsTrue(true);
            }
            
        }
        [TestMethod()]
        public void loginTest2()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            SubscribedUser sub1 = new SubscribedUser("Danny", dbsubscribedUser.encryptPassword("Shovevani"), new ShoppingBasket());
            dbsubscribedUser.register(sub1);

            Session session = new Session();
            UserState state = session.getState();
            try
            {

                state.login("Danny", "Shovevani", session);
                Assert.IsTrue(session.getState() is LoggedIn);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

        }
        [TestMethod()]
        public void loginTest3()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            SubscribedUser sub1 = new SubscribedUser("Danny", dbsubscribedUser.encryptPassword("Shovevani"), new ShoppingBasket());
            dbsubscribedUser.register(sub1);

            Session session = new Session();
            UserState state = session.getState();
            Session session2 = new Session();
            UserState state2 = session2.getState();
            try
            {
                state.login("Danny", "aaaa", session2);
                Assert.Fail();
            }
            catch(LoginException e)
            {
                Assert.IsTrue(true);
                
            }
            
            
        }

        [TestMethod()]
        public void logoutTest()
        {
            UserState state = new Guest();
            try
            {
                state.logout(null, null);
                Assert.Fail();
            }
            catch (UserStateException)
            {
                Assert.IsTrue(true);
            }


        }

        [TestMethod()]
        public void registerTest1()
        {
            DBSubscribedUser dbsubscribedUser = DBSubscribedUser.getInstance();
            SubscribedUser sub3 = new SubscribedUser("Benny", "aaaa", new ShoppingBasket());
            dbsubscribedUser.register(sub3);

            Session session = new Session();
            UserState state = session.getState();
            try
            {
                state.register("Benny", "aaaa", session);
                Assert.Fail();
            }
            catch (RegisterException)
            {
                Assert.IsTrue(state is Guest);
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
            catch (UserStateException e)
            {
                Assert.IsTrue(true);
            }
            
        }
    }
}

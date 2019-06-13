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

        [TestInitialize()]
        public void init()
        {
            MarketSystem.initTestWitOutRead();
        }
        [TestMethod()]
        public void createStoreTest()
        {
            UserState state = new Admin();
            state.createStore("ToyRUs", "lots of toys", new SubscribedUser("aa", "aa", null));
        }

    

        [TestMethod()]
        public void loginTest()
        {
            try
            {
                Admin admin = new Admin();
                admin.login("u1", "123", session);
                Assert.Fail();
            }
            catch (LoginException)
            {
                Assert.IsTrue(true);
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
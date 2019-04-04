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
        [TestMethod()]
        public void AdminTest()
        {

        }

        [TestMethod()]
        public void createStoreTest()
        {
            UserState state = new Admin();
            Assert.IsNull(state.createStore("ToyRUs", "lots of toys", null));
        }

        [TestMethod()]
        public void closeStoreTest()
        {
            Assert.Fail();
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
            UserState state = new Admin();
            Assert.IsTrue(Equals(state.login("shalom", "1111", null), "ERROR: User already logged in"));
        }

        [TestMethod()]
        public void logoutTest()
        {
            UserState state = new Admin();
        }

        [TestMethod()]
        public void registerTest()
        {
            UserState state = new Admin();
            Assert.IsTrue(Equals(state.register("shalom", "1111", null), "ERROR: User already registered"));
        }

        [TestMethod()]
        public void removeUserTest()
        {
            Assert.Fail();
        }
    }
}

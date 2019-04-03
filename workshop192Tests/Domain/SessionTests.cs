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
    public class SessionTests
    {
        [TestMethod()]
        public void SessionTest()
        {
            Assert.Fail();
        }


        [TestMethod()]
        public void getStateTest()
        {
            //return this.userState;
        }

        [TestMethod()]
        public void getSubscribedUserTest()
        {
            //return this.subscribedUser;
        }
        [TestMethod()]
        public void getShoppingBasketTest()
        {
            //return this.shoppingBasket;
        }
        [TestMethod()]
        public void setSubscribedUserTest()
        {
            //this.subscribedUser = sub;
        }
        [TestMethod()]
        public void setStateTest()
        {
            //this.userState = state;
        }
        [TestMethod()]
        public void setShoppingBasketTest()
        {
            //this.shoppingBasket = shoppingB;
        }
        [TestMethod()]
        public void loginTest()
        {
           // return userState.login(username, password, this);
        }
        [TestMethod()]
        public void registerTest()
        {
            //return userState.register(username, password, this);
        }
        [TestMethod()]
        public void logoutTest()
        {
           // return userState.logout(subscribedUser, this);
        }
        [TestMethod()]
        public void getPurchaseHistoryTest()
        {
           // return userState.getPurchaseHistory(subscribedUser);
        }
        [TestMethod()]
        public void createStoreTest(int id, String storeName, String description)
        {
           // return userState.createStore(id, storeName, description);
        }
        [TestMethod()]
        public void closeStoreTest(int id)
        {
           // return userState.closeStore(id);
        }
        [TestMethod()]
        public void removeUserTest(String username)
        {
            //return userState.removeUser(username);
        }
    }
}
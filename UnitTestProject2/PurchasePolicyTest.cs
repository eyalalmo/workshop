using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;
using workshop192.ServiceLayer;

namespace workshop192.ServiceLayer.Tests
{
    [TestClass()]
    class PurchasePolicyTest
    {
        private UserService userService = UserService.getInstance();
        private StoreService storeService = StoreService.getInstance();
        private BasketService basketService = BasketService.getInstance();
        int session1;
        int session2;
        int store1;
        int p1;

        int bamba;
        int bisli;

        [TestInitialize()]
        public void initial()
        {
            userService.setup();
            DBProduct.getInstance().init();
            DBStore.getInstance().init();
            session1 = userService.startSession();// login 
            userService.register(session1, "user1", "user1");
            userService.login(session1, "user1", "user1");
            store1 = storeService.addStore("Makolet", "groceryStore", session1);

            p1 = storeService.addProduct("shirt", "clothing", 50, 4, 4, store1, session1);
            bamba = storeService.addProduct("bamba", "food", 15, 5, 17, store1, session1);
            bisli = storeService.addProduct("bisli", "food", 20, 4, 50, store1, session1);

            session2 = userService.startSession();// login 
            userService.register(session1, "user2", "user2");
            userService.login(session1, "user2", "user2");
        }
        [TestMethod]
        public void conflictionPurchasePolicy()
        {
            try
            {
                storeService.setMaxAmountPolicy(store1, session1, 4);
                storeService.setMinAmountPolicy(store1, session1, 6);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }
        /*
        [TestMethod]
        public void MoreThanTwoPurchasePolicies()
        {
            try
            {
                storeService.addMaxAmountPolicy(store1, session1, 4);
                storeService.addMinAmountPolicy(store1, session1, 2);
                storeService.addMaxAmountPolicy(store1, session1, 3);
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }

        }
        [TestMethod]
        public void TwoPoliciesSameType()
        {
            try
            {
                storeService.addMaxAmountPolicy(store1, session1, 4);
                storeService.addMaxAmountPolicy(store1, session1, 3);
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }

        }
        */
        [TestMethod]
        public void checkPurchasePolicyFail()
        {
            try
            {
                storeService.setMaxAmountPolicy(store1, session1, 4);
                storeService.setMinAmountPolicy(store1, session1, 3);
                basketService.addToCart(session2, bamba, 2);
                basketService.addToCart(session2, bisli, 2);
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }


    }
}


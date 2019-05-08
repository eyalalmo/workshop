using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;
using workshop192.ServiceLayer;

namespace UnitTestProject2
{
    [TestClass()]
    class DiscountTests
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
            bisli = storeService.addProduct("bisli", "food", 20,4,50,store1, session1);

            session2 = userService.startSession();// login 
            userService.register(session1, "user2", "user2");
            userService.login(session1, "user2", "user2");
        }

        [TestMethod]
        public void AddVisibleDiscount()
        {
            storeService.addProductVisibleDiscount(p1, session1, 0.1, "1 month");
            basketService.addToCart(session1, p1, 3);
            ShoppingCart sc = basketService.getCart(session1, store1);
            double actualPrice = sc.getTotalAmount();
            Assert.AreEqual(actualPrice, 135);
        }
        [TestMethod]
        public void ReliantDiscountSuccessOneProduct()
        {
            storeService.addReliantDiscountSameProduct(store1, session1, 0.25, "1 month", 3, bamba);
            // above 3 bambas get 25 % of
            basketService.addToCart(session1, bamba, 5);
            ShoppingCart sc = basketService.getCart(session1, store1);
            double actualPrice = sc.getTotalAmount();
            Assert.AreEqual(actualPrice, 3 * 0.75 * 15);
        }
        [TestMethod]
        public void ReliantDiscountFailOneProduct()
        {
            storeService.addReliantDiscountSameProduct(store1, session1, 0.25, "1 month", 3, bamba);
            basketService.addToCart(session1, bamba, 1);
            ShoppingCart sc = basketService.getCart(session1, store1);
            double actualPrice = sc.getTotalAmount();
            Assert.AreNotEqual(actualPrice, 0.75 * 15);
            Assert.AreEqual(actualPrice, 15);
        }
        public void ReliantDiscountSuccessTotalAmount()
        {
            storeService.addReliantDiscountTotalAmount(store1, session1, 0.4, "a month", 400);
            // total cart above 400 shekels get 40 % off
            basketService.addToCart(session1, bamba, 14);
            basketService.addToCart(session1, bisli, 30);
            ShoppingCart sc = basketService.getCart(session1, store1);
            double actualPrice = sc.getTotalAmount();
            double expected = 0.4 * (14 * 15 + 30 * 20);
            Assert.AreEqual(actualPrice, expected);
        }
        [TestMethod]
        public void ReliantDiscountFailTotalAmount()
        {
            storeService.addReliantDiscountTotalAmount(store1, session1, 0.4, "a month", 400);
            // total cart above 400 shekels get 40 % off
            basketService.addToCart(session1, bamba, 2);
            basketService.addToCart(session1, bisli, 2);
            ShoppingCart sc = basketService.getCart(session1, store1);
            double actualPrice = sc.getTotalAmount();
            double expected = (2 * 15 + 2 * 20);
            Assert.AreEqual(actualPrice, expected);
        }
        [TestMethod]
        public void CouponSuccess()
        {
            storeService.addCouponToStore(session1, store1, "oshim3", 0.2, "a month");

            basketService.addToCart(session2, bamba, 2);
            basketService.addToCart(session2, bisli, 2);
            basketService.addCouponToCart(session2, store1, "oshim3");
            Dictionary<int, ShoppingCart> shoppingCarts = basketService.getShoppingCarts(session2);
            ShoppingCart sc = shoppingCarts[store1];

            Assert.AreEqual(sc.getTotalAmount(), 56);
        }

        [TestMethod]
        public void InvisibleCouponFail()
        {
            storeService.addCouponToStore(session1, store1, "oshim3", 0.2, "a month");

            basketService.addToCart(session2, bamba, 2);
            basketService.addToCart(session2, bisli, 2);
            basketService.addCouponToCart(session2, store1, "notOshim");
            Dictionary<int, ShoppingCart> shoppingCarts = basketService.getShoppingCarts(session2);
            ShoppingCart sc = shoppingCarts[store1];

            Assert.AreEqual(sc.getTotalAmount(), 70);
        }
    }
}

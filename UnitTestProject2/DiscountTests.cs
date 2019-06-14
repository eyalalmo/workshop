using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;

namespace workshop192.ServiceLayer.Tests
{
    [TestClass]
    public class DiscountTests
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
            DBProduct.getInstance().initTests();
            DBStore.getInstance().initTests();
            session1 = userService.startSession();// login 
            try
            {
                userService.register(session1, "user23", "user23");
            }
            catch (Exception) { }
            userService.login(session1, "user23", "user23");
            store1 = storeService.addStore("Makolet", "groceryStore", session1);

            p1 = storeService.addProduct("shirt", "clothing", 50, 4, 4, store1, session1);
            bamba = storeService.addProduct("bamba", "food", 15, 5, 17, store1, session1);
            bisli = storeService.addProduct("bisli", "food", 20, 4, 50, store1, session1);

            session2 = userService.startSession();// login 
            userService.register(session2, "user7", "user7");
            userService.login(session2, "user7", "user7");
        }
        [TestMethod]
        public void AddVisibleDiscount()
        {
            storeService.addProductVisibleDiscount(p1, session1, 0.1, "1 month");
            basketService.addToCart(session1, p1, 3);
            double actualPrice = basketService.getAmountOfCart(store1, session1);
            Assert.AreEqual(actualPrice, 135);
        }
        [TestMethod]
        public void ReliantDiscountSuccessOneProduct()
        {
            storeService.addReliantDiscountSameProduct(store1, session1, 0.25, "1 month", 3, bamba);
            // above 3 bambas get 25 % of
            basketService.addToCart(session1, bamba, 5);
            double actualPrice = basketService.getAmountOfCart(store1, session1);
            Assert.AreEqual(actualPrice, 5 * 0.75 * 15);
        }
        [TestMethod]
        public void ReliantDiscountFailOneProduct()
        {
            storeService.addReliantDiscountSameProduct(store1, session1, 0.25, "1 month", 3, bamba);
            basketService.addToCart(session1, bamba, 1);
            double actualPrice = basketService.getAmountOfCart(store1, session1);
            Assert.AreNotEqual(actualPrice, 0.75 * 15);
            Assert.AreEqual(actualPrice, 15);
        }
        public void ReliantDiscountSuccessTotalAmount()
        {
            storeService.addReliantDiscountTotalAmount(store1, session1, 0.4, "a month", 400);
            // total cart above 400 shekels get 40 % off
            basketService.addToCart(session1, bamba, 14);
            basketService.addToCart(session1, bisli, 30);
            double actualPrice = basketService.getAmountOfCart(store1, session1);

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
            double actualPrice = basketService.getAmountOfCart(store1, session1);
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
            double actualPrice = basketService.getAmountOfCart(store1, session2);
            Assert.AreEqual(actualPrice, 56);
        }

        [TestMethod]
        public void InvisibleCouponFail()
        {
            try {
                storeService.addCouponToStore(session1, store1, "oshim3", 0.2, "a month");

                basketService.addToCart(session2, bamba, 2);
                basketService.addToCart(session2, bisli, 2);
                basketService.addCouponToCart(session2, store1, "notOshim");
                //double actualPrice = basketService.getAmountOfCart(store1, session2);

                //Assert.AreEqual(actualPrice, 70);
                Assert.Fail();
            }
            catch(ArgumentException)
            {
                Assert.IsTrue(true);
            }


            }
        [TestMethod]
        public void addComplexSuccess()
        {
            try
            {
                ReliantDiscountSuccessOneProduct();
                ReliantDiscountSuccessOneProduct();
                storeService.complexDiscount("0 1", store1, "and", 0.3, "12/12/2020", session1);
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            } 
        }
        [TestMethod]
        public void addComplex()
        {
            try
            {
                ReliantDiscountSuccessOneProduct();
                ReliantDiscountSuccessOneProduct();
                storeService.complexDiscount("0 1", store1, "and", 0.3, "12/12/2020", session1);
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

    }


}

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
            userService.testSetup();
            //DBProduct.getInstance().initTests();
            //DBStore.getInstance().initTests();
            session1 = userService.startSession();
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
            storeService.addProductVisibleDiscount(p1, session1, 0.1, "12/12/2020");
            basketService.addToCart(session1, p1, 3);
            double actualPrice = basketService.getAmountOfCart(store1, session1);
            Assert.AreEqual(actualPrice, 135);
        }
        [TestMethod]
        public void ReliantDiscountSuccessOneProduct()
        {
            storeService.addReliantDiscountSameProduct(store1, session1, 0.25, "12/12/2020", 3, bamba);
            // above 3 bambas get 25 % of
            basketService.addToCart(session1, bamba, 5);
            double actualPrice = basketService.getAmountOfCart(store1, session1);
            Assert.AreEqual(actualPrice, 5 * 0.75 * 15);
        }
      
        [TestMethod]
        public void ReliantDiscountFailOneProduct()
        {
            storeService.addReliantDiscountSameProduct(store1, session1, 0.25, "12/12/2020", 3, bamba);
            basketService.addToCart(session1, bamba, 1);
            double actualPrice = basketService.getAmountOfCart(store1, session1);
            Assert.AreNotEqual(actualPrice, 0.75 * 15);
            Assert.AreEqual(actualPrice, 15);
        }
        public void ReliantDiscountSuccessTotalAmount()
        {
            storeService.addReliantDiscountTotalAmount(store1, session1, 0.4, "12/12/2020", 400);
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
            storeService.addReliantDiscountTotalAmount(store1, session1, 0.4, "12/12/2020", 400);
            // total cart above 400 shekels get 40 % off
            basketService.addToCart(session1, bamba, 2);
            basketService.addToCart(session1, bisli, 2);
            double actualPrice = basketService.getAmountOfCart(store1, session1);
            double expected = (2 * 15 + 2 * 20);
            Assert.AreEqual(actualPrice, expected);
        }
        [TestMethod]
        public void addComplexSuccess()
        {
            try
            {
                storeService.addReliantDiscountSameProduct(store1, session1, 0.25, "12/12/2020", 3, bisli);
                storeService.addReliantDiscountSameProduct(store1, session1, 0.25, "12/12/2020", 3, bamba);
                basketService.addToCart(session1, bamba, 4);
                basketService.addToCart(session1, bisli, 4);
                storeService.complexDiscount("0 1", store1, "and", 0.3, "12/12/2020", session1);
                double actualPrice = basketService.getAmountOfCart(store1, session1);
                double expected = ((4 * 15)*0.75 + (4 * 20)*0.75);
                Assert.AreEqual(expected,actualPrice);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
        [TestMethod]
        public void addComplexFail()
        {
            try
            {
                storeService.addReliantDiscountSameProduct(store1, session1, 0.25, "12/12/2020", 3, bisli);
                storeService.addReliantDiscountSameProduct(store1, session1, 0.25, "12/12/2020", 3, bamba);
                storeService.complexDiscount("0 1", store1, "aaa", 0.3, "12/12/2020", session1);
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }



    }


}

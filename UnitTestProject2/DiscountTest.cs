using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;
using workshop192.ServiceLayer;

namespace UnitTestProject3
{
    [TestClass()]
    class DiscountTests
    {
        private UserService userService = UserService.getInstance();
        private StoreService storeService = StoreService.getInstance();
        private BasketService basketService = BasketService.getInstance();
        int session1;
        int store1;
        int p1;

        int bamba;
       // int bisli;

        [TestInitialize()]
        public void initial()
        {
            userService.setup();
            DBProduct.getInstance().init();
            DBStore.getInstance().init();
            session1 = userService.startSession();// login 
            userService.register(session1, "user1", "user1");
            userService.login(session1, "user1", "user1");
            store1= storeService.addStore("Makolet", "groceryStore", session1);
            p1= storeService.addProduct("shirt", "clothing", 50, 4, 4, store1, session1);
            bamba = storeService.addProduct("bamba", "food", 15, 5, 17, store1, session1);
        }

        [TestMethod]
        public void AddVisibleDiscount()
        {
            userService.addProductVisibleDiscount(p1, session1, 0.1, "1 month");
            basketService.addToCart(session1, p1, 3);
            ShoppingCart sc = basketService.getCart(session1, store1);
            double actualPrice = sc.getTotalAmount();
            Assert.AreEqual(actualPrice, 135);
        }
        public void ReliantDiscountSuccessOneProduct()
        {
            userService.addReliantDiscountSameProduct(store1, session1, 0.25, "1 month", 3, bamba);
            // above 3 bambas get 25 % of
            basketService.addToCart(session1, bamba, 5);
            ShoppingCart sc = basketService.getCart(session1, store1);
            double actualPrice = sc.getTotalAmount();
            Assert.AreEqual(actualPrice, 3 * 0.75 * 15);
        }
    }
}

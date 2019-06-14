using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;
using workshop192.ServiceLayer;

namespace UnitTests.Domain
{
    [TestClass]
    public class DBDiscountTests
    {
        [TestClass]
        public class DiscountTests
        {
            private DBDiscount dbdiscount = DBDiscount.getInstance();
            private UserService userService = UserService.getInstance();

            Store store1;
            Product p1;

            Product bamba;
            Product bisli;

            [TestInitialize()]
            public void initial()
            {
                userService.testSetup();
                //DBProduct.getInstance().initTests();
                //DBStore.getInstance().initTests();
                store1 = new Store("Makolet", "groceryStore");
               
                p1 = new Product("shirt", "clothing", 50, 4, 4, store1);
                store1.addProduct(p1);
                bamba = new Product("bamba", "food", 15, 5, 17, store1);
                store1.addProduct(bamba);
                bisli = new Product("bisli", "food", 20, 4, 50, store1);
                store1.addProduct(bisli);
            }
            [TestMethod]
            public void AddDiscount()
            {
                try
                {
                    VisibleDiscount v = new VisibleDiscount(0.2, "12/12/2020", "StoreVisibleDiscount", store1.getStoreID());
                    dbdiscount.addDiscount(v);
                    DiscountComponent d = dbdiscount.getDiscountByID(v.getId());
                    Assert.IsTrue(d.getId() == v.getId());

                }
                catch (Exception)
                {
                    Assert.Fail();
                }

            }

        }
    }
}


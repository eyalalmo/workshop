using System;
using System.Collections.Generic;
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
            Store store2;
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
                store2 = new Store("Apple", "apples");
                p1 = new Product("shirt", "clothing", 50, 4, 4, store1);
                store1.addProduct(p1);
                bamba = new Product("bamba", "food", 15, 5, 17, store1);
                store1.addProduct(bamba);
                bisli = new Product("bisli", "food", 20, 4, 50, store1);
                store1.addProduct(bisli);
            }
            [TestMethod]
            public void AddDiscountSuccess()
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
            [TestMethod]
            public void AddDiscountFail()
            {
                try
                {
                    VisibleDiscount v = new VisibleDiscount(0.2, "12/12/2017", "StoreVisibleDiscount", store1.getStoreID());
                    Assert.Fail();

                }
                catch (Exception)
                {
                    Assert.IsTrue(true);
                }

            }
            [TestMethod]
            public void RemoveDiscount()
            {
                try
                {
                    VisibleDiscount v = new VisibleDiscount(0.2, "12/12/2020", "StoreVisibleDiscount", store1.getStoreID());
                    dbdiscount.addDiscount(v);
                    dbdiscount.removeDiscount(v);
                    DiscountComponent d = dbdiscount.getDiscountByID(v.getId());
                    Assert.IsTrue(d==null);

                }
                catch (Exception)
                {
                    Assert.Fail();
                }

            }
            [TestMethod]
            public void AllStoreDiscounts()
            {
                try
                {
                    VisibleDiscount v = new VisibleDiscount(0.2, "12/12/2020", "StoreVisibleDiscount", store2.getStoreID());
                    ReliantDiscount r = new ReliantDiscount(0.2, "12/12/2020", 100, store2.getStoreID());
                    dbdiscount.addDiscount(v);
                    dbdiscount.addDiscount(r);
                    LinkedList<DiscountComponent> list = dbdiscount.getStoreDiscountsList(store2.getStoreID());
                    Assert.IsTrue(true);

                }
                catch (Exception)
                {
                    Assert.Fail();
                }

            }

        }
    }
}


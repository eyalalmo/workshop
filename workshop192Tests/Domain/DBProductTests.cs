using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.ServiceLayer;

namespace workshop192.Domain.Tests
{
    [TestClass()]
    public class DBProductTests
    {
        public int p,p1,p2,p3,p4;
        private StoreService storeService = StoreService.getInstance();
        private UserService userService = UserService.getInstance();
        public int session1;

        int storeID;
        [TestInitialize()]
        public void TestInitialize()
        {
            UserService.getInstance().setup();

            session1 = userService.startSession();
            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            DBProduct.getInstance().initDB();

            storeID = storeService.addStore("myStore", "the best store ever", session1);
            p = storeService.addProduct("pizza", "food", 40, 0, 10, storeID, session1);

            p1 = storeService.addProduct("fries", "food", 20, 2, 10, storeID, session1);
            p2 = storeService.addProduct("coca cola", "drinks", 8, 2, 10, storeID, session1);
            p3 = storeService.addProduct("ketchup", "sauces", 2, 0, 10, storeID, session1);
            p4 = storeService.addProduct("coca cola zero", "drinks", 8, 4, 10, storeID, session1);
        }

        [TestMethod()]
        public void addProductTest()
        {
            Product prod1 = DBProduct.getInstance().getProductByID(p);

            Assert.AreEqual(DBProduct.getInstance().getProductByID(p), prod1);
        }

        [TestMethod()]
        public void removeProductTest()
        {
            try
            {
                addProductTest();
                Product prod1 = DBProduct.getInstance().getProductByID(p);

                DBProduct.getInstance().removeProduct(prod1);
                Assert.IsTrue(true);
            }
            catch (Exception) {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void removeNonExistingProductTest()
        {
            try
            {
                Product prod1 = DBProduct.getInstance().getProductByID(p);

                DBProduct.getInstance().removeProduct(prod1);
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }


        [TestMethod()]
        public void searchProductsByNameTest()
        {
           /* DBProduct.getInstance().addProduct(p);
            DBProduct.getInstance().addProduct(p1);
            DBProduct.getInstance().addProduct(p2);
            DBProduct.getInstance().addProduct(p3);
            DBProduct.getInstance().addProduct(p4);
    */        
            List<Product> result1 = DBProduct.getInstance().searchProducts("pizza", null, null);

            Product prod1 = DBProduct.getInstance().getProductByID(p);
            Product prod2 = DBProduct.getInstance().getProductByID(p1);

            Assert.IsTrue(result1.Contains(prod1));
            Assert.IsFalse(result1.Contains(prod2));
        }
        [TestMethod()]
        public void searchProductsByCategoryTest()
        {
            /*
            DBProduct.getInstance().addProduct(p);
            DBProduct.getInstance().addProduct(p1);
            DBProduct.getInstance().addProduct(p2);
            DBProduct.getInstance().addProduct(p3);
            DBProduct.getInstance().addProduct(p4);
            */
            List<Product> result1 = DBProduct.getInstance().searchProducts(null, null, "food");
             Product prod1 = DBProduct.getInstance().getProductByID(p);
            Product prod2 = DBProduct.getInstance().getProductByID(p1);

            Assert.IsTrue(result1.Contains(prod1));
            Assert.IsTrue(result1.Contains(prod2));
        }
        [TestMethod()]
        public void searchProductsByCategoryFilterByPriceRangeTest()
        {
            /*
            DBProduct.getInstance().addProduct(p);
            DBProduct.getInstance().addProduct(p1);
            DBProduct.getInstance().addProduct(p2);
            DBProduct.getInstance().addProduct(p3);
            DBProduct.getInstance().addProduct(p4);
    */        
            int[] range = { 30, 50 };
            List<Product> result1 = DBProduct.getInstance().searchProducts(null, null, "food");
            List<Product> result2 = DBProduct.getInstance().filterBy(result1, range, 0);
            Product prod1 = DBProduct.getInstance().getProductByID(p);
            Product prod2 = DBProduct.getInstance().getProductByID(p1);

            Assert.IsTrue(result2.Contains(prod1));
            Assert.IsFalse(result2.Contains(prod2));
        }
        [TestMethod()]
        public void searchProductsByKeywordsTest()
        {
            /*DBProduct.getInstance().addProduct(p);
            DBProduct.getInstance().addProduct(p1);
            DBProduct.getInstance().addProduct(p2);
            DBProduct.getInstance().addProduct(p3);
            DBProduct.getInstance().addProduct(p4);
            */
            List<Product> result1 = DBProduct.getInstance().searchProducts(null, "coca",null);

            Product prod1 = DBProduct.getInstance().getProductByID(p2);
            Product prod2 = DBProduct.getInstance().getProductByID(p4);

            Assert.IsTrue(result1.Contains(prod1));
            Assert.IsTrue(result1.Contains(prod2));
        }
        [TestMethod()]
        public void searchProductsByKeywordsFilterByRankTest()
        {
            /* DBProduct.getInstance().addProduct(p);
             DBProduct.getInstance().addProduct(p1);
             DBProduct.getInstance().addProduct(p2);
             DBProduct.getInstance().addProduct(p3);
             DBProduct.getInstance().addProduct(p4);

     */
            List<Product> result1 = DBProduct.getInstance().searchProducts(null, "coca", null);
            List<Product> result2 = DBProduct.getInstance().filterBy(result1, null, 3);
            Product prod1 = DBProduct.getInstance().getProductByID(p2);
            Product prod2 = DBProduct.getInstance().getProductByID(p4);

            Assert.IsFalse(result2.Contains(prod1));
            Assert.IsTrue(result2.Contains(prod2));
        }



    }
}
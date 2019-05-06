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
    public class DBProductTests
    {
        Product p,p1,p2,p3,p4;
        Store s = null;
        [TestInitialize()]
        public void TestInitialize()
        {
            DBProduct.getInstance().initDB();
            p = new Product("pizza", "food", 40, 0, 10, s);
            p1 = new Product("fries", "food", 20, 2, 10, s);
            p2 = new Product("coca cola", "drinks", 8, 2, 10, s);
            p3 = new Product("ketchup", "sauces", 2, 0, 10, s);
            p4 = new Product("coca cola zero", "drinks", 8, 4, 10, s);
        }

        [TestMethod()]
        public void addProductTest()
        {
            int ID = DBProduct.getInstance().addProduct(p);
            Assert.AreEqual(DBProduct.getInstance().getProductByID(ID), p);
        }

        [TestMethod()]
        public void removeProductTest()
        {
            try
            {
                addProductTest();
                DBProduct.getInstance().removeProduct(p);
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
                DBProduct.getInstance().removeProduct(p);
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
            DBProduct.getInstance().addProduct(p);
            DBProduct.getInstance().addProduct(p1);
            DBProduct.getInstance().addProduct(p2);
            DBProduct.getInstance().addProduct(p3);
            DBProduct.getInstance().addProduct(p4);
            List<Product> result1 = DBProduct.getInstance().searchProducts("pizza", null, null);
            Assert.IsTrue(result1.Contains(p));
            Assert.IsFalse(result1.Contains(p1));
        }
        [TestMethod()]
        public void searchProductsByCategoryTest()
        {
            DBProduct.getInstance().addProduct(p);
            DBProduct.getInstance().addProduct(p1);
            DBProduct.getInstance().addProduct(p2);
            DBProduct.getInstance().addProduct(p3);
            DBProduct.getInstance().addProduct(p4);
            List<Product> result1 = DBProduct.getInstance().searchProducts(null, null, "food");
            Assert.IsTrue(result1.Contains(p));
            Assert.IsTrue(result1.Contains(p1));
        }
        [TestMethod()]
        public void searchProductsByCategoryFilterByPriceRangeTest()
        {
            DBProduct.getInstance().addProduct(p);
            DBProduct.getInstance().addProduct(p1);
            DBProduct.getInstance().addProduct(p2);
            DBProduct.getInstance().addProduct(p3);
            DBProduct.getInstance().addProduct(p4);
            int[] range = { 30, 50 };
            List<Product> result1 = DBProduct.getInstance().searchProducts(null, null, "food");
            List<Product> result2 = DBProduct.getInstance().filterBy(result1, range, 0);
            Assert.IsTrue(result2.Contains(p));
            Assert.IsFalse(result2.Contains(p1));
        }
        [TestMethod()]
        public void searchProductsByKeywordsTest()
        {
            DBProduct.getInstance().addProduct(p);
            DBProduct.getInstance().addProduct(p1);
            DBProduct.getInstance().addProduct(p2);
            DBProduct.getInstance().addProduct(p3);
            DBProduct.getInstance().addProduct(p4);
            List<Product> result1 = DBProduct.getInstance().searchProducts(null, "coca",null);
            Assert.IsTrue(result1.Contains(p2));
            Assert.IsTrue(result1.Contains(p4));
        }
        [TestMethod()]
        public void searchProductsByKeywordsFilterByRankTest()
        {
            DBProduct.getInstance().addProduct(p);
            DBProduct.getInstance().addProduct(p1);
            DBProduct.getInstance().addProduct(p2);
            DBProduct.getInstance().addProduct(p3);
            DBProduct.getInstance().addProduct(p4);
            List<Product> result1 = DBProduct.getInstance().searchProducts(null, "coca", null);
            List<Product> result2 = DBProduct.getInstance().filterBy(result1, null, 3);
            Assert.IsFalse(result2.Contains(p2));
            Assert.IsTrue(result2.Contains(p4));
        }



    }
}
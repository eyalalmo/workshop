using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;
using workshop192.ServiceLayer;



namespace UnitTestProject2
{
    [TestClass]
    public class UserServiceTests
    {
        //2.2+2.3
        [TestMethod]
        public void TestMethod1()
        {
            UserService user = UserService.getInstance();
            Session session =  user.startSession();
            String str=  user.register(session, "user", "user");
            Assert.AreEqual(str, "");
            Assert.AreEqual(user.login(session, "user", "user"), "");
            Assert.IsTrue(session.getSubscribedUser()!=null);
            Assert.IsTrue(session.getState() is LoggedIn);
            //alternative
            Assert.AreNotEqual(user.login(session,"user","user"),"");
            Assert.IsTrue(session.getState() is LoggedIn);
            Assert.AreNotEqual(user.login(session,"bbb","aaaa"),"");
            Assert.AreNotEqual(user.register("user", "user"),"");
        }

        //2.5.1+2.5.2
        [TestMethod]
        public void TestMethod2()
        {
            UserService userService = UserService.getInstance();
            StoreService storeService = StoreService.getInstance();
            Session session =  userService.startSession();
            Store store1 = storeService.addStore();
            Store store2 = storeService.addStore();
            storeService.addProduct("pan","kitchen", 100, 2, 4, store1);
            storeService.addProduct("stove","kitchen", 200, 3, 2, store1);
            storeService.addProduct("shirt","clothing", 20, 5, 2, store1);
            storeService.addProduct("dog food","dog section", 120, 1, 2, store2);
            storeService.addProduct("cat food","cat section", 110, 4, 2, store2);

            List<Product> searchResult1 = userService.searchProducts(null, null, "kitchen");
            Assert.IsTrue(searchResult1.Count==2);
            Product p1 = searchResult1[0];
            Product p2 = searchResult1[1];
            Boolean contains= (Equals(p1.getProductName(), "pan")&&Equals(p2.getProductName(), "stove"))||(Equals(p1.getProductName(), "stove")&&Equals(p2.getProductName(), "pan"));
            Assert.IsTrue(contains);

        }

    }
}

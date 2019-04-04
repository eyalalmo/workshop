using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;
using workshop192.ServiceLayer;



namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            UserService user = UserService.getInstance();
            Session session =  user.startSession();
            string str=  user.register(session, "user", "user");
            Assert.AreEqual(str, "");

        }

    }
}

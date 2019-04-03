using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.Domain;




namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {


       // •	משתמש בוחר מוצר/ים ומעביר אותו/ם לעגלת הקניות.
      //  •	המשתמש נכנס לעגלת הקניות ומבצע את הרכישה(תיאור הרכישה מתואר בתרחיש השימוש 2.8.1).


        [TestMethod]
        public void TestMethod1()
        {
            Session user = new Session();
            user.register("user", "user");
            user.login("user", "user");
            user.createStore("user store", "the best store", user.getSubscribedUser());
            
        }
    }
}

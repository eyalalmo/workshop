using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.ServiceLayer.Tests
{
    [TestClass()]
    public class StoreServiceTests
    {
        //3.2
        [TestMethod()]
        public void TestMethod1()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session = new Session();

            userService.register(session, "anna", "banana");
            userService.login(session, "anna", "banana");
           // Store store = 
        }

       
    }
}
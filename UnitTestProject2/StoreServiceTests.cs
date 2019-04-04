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

        //4.3
        [TestMethod()]
        public void addMannagerByAnOwner1()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();

            userService.register(session1, "anna", "banana");
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani", "123");
            
            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreEqual(storeService.addManager(store, "dani", true, true, true, session1), "");
            Assert.AreEqual(storeService.addProduct("myProduct", "some category", 10, 0, 10, store, session2), "");
        }

        //4.3
        [TestMethod()]
        public void addMannagerByAnOwner2()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();

            userService.register(session1, "anna", "banana");
            userService.login(session1, "anna", "banana");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreNotEqual(storeService.addManager(store, "dani", true, true, true, session1), "");
        }

        //4.3
        [TestMethod()]
        public void addMannagerByAnOwner3()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();

            userService.register(session1, "anna", "banana");
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            storeService.addManager(store, "dani", true, true, true, session1);
            Assert.AreNotEqual(storeService.addManager(store, "dani", true, true, true, session1), "");
        }

       
    }
}
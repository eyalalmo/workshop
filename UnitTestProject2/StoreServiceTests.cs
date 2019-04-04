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

        [TestInitialize]
        public void init()
        {
;
        }
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

       // 4.4
        [TestMethod()]
        public void removeOwnerTest()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();
            Session session4 = new Session();

            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");
        
            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            userService.register(session3, "dani2", "123");
            userService.register(session4, "dani3", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreEqual("", storeService.addOwner(store, "dani1", session1));
            Assert.AreEqual("", storeService.addOwner(store, "dani2", session2));
            Assert.AreEqual("", storeService.addOwner(store, "dani3", session2));
            Assert.AreEqual(storeService.removeRole(store, "dani1", session1),"");
            foreach (StoreRole role in store.getRoles())
            {
                if(role.getAppointedBy()==session1.getSubscribedUser())
                {
                    Assert.Fail();
                 }
            }
    
        }
        // 4.4
        [TestMethod()]
        public void removeOwnerTestFaild1()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();
            Session session4 = new Session();

            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            userService.register(session3, "dani2", "123");
            userService.register(session4, "dani3", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreEqual("", storeService.addOwner(store, "dani1", session1));
            Assert.AreEqual("", storeService.addOwner(store, "dani2", session2));
            Assert.AreEqual("", storeService.addOwner(store, "dani3", session2));
            Assert.AreNotEqual(storeService.removeRole(store, "dani1", session2), "");
        
        }

        // 4.4
        [TestMethod()]
        public void removeOwnerTestFaild2()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();
            Session session4 = new Session();

            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            userService.register(session3, "dani2", "123");
            userService.register(session4, "dani3", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreEqual("", storeService.addOwner(store, "dani1", session1));
            Assert.AreEqual("", storeService.addOwner(store, "dani2", session2));
            Assert.AreNotEqual(storeService.removeRole(store, "dani3", session2), "");
        }


        // 4.6
        [TestMethod()]
        public void removeManagerTest()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();

            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreEqual("", storeService.addManager(store, "dani1",true,true,true, session1));
            Assert.AreEqual(storeService.removeRole(store, "dani1", session1), "");
        }
        // 4.6
        [TestMethod()]
        public void removeManagerTestFaild1()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();
            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            userService.register(session3, "dani2", "123");
            userService.login(session3, "dani2", "123");

            Store store = storeService.addStore("myStore", "the best store ever", session1);
            
            Assert.AreEqual("", storeService.addOwner(store, "dani2", session1));

            Assert.AreEqual("", storeService.addManager(store, "dani1", true, true, true, session1));
            Assert.AreNotEqual(storeService.removeRole(store, "dani1", session3), "");
        }
        // 4.6
        [TestMethod()]
        public void removeManagerTestFaild2()
        {
            StoreService storeService = StoreService.getInstance();
            UserService userService = UserService.getInstance();
            Session session1 = new Session();
            Session session2 = new Session();
            Session session3 = new Session();
            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");
            Store store = storeService.addStore("myStore", "the best store ever", session1);
            Assert.AreNotEqual(storeService.removeRole(store, "dani1", session1), "");
        }




    }
}
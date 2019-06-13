
using workshop192.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace workshop192.Domain.Tests
{
    [TestClass()]
    public class DBSessionTests
    {
        [TestInitialize()]
        public void init()
        {
            MarketSystem.initTestWitOutRead();
        }
        [TestMethod()]
        public void addSessionTest()
        {
            DBSession db = DBSession.getInstance();
            db.initSession();
            db.generate();
            //Assert.AreEqual(db.getSession(s), "");
        }

        [TestMethod()]
        public void removeSessionTest()
        {
            DBSession db = DBSession.getInstance();
            db.initSession();
            db.removeSession(db.generate());
            //Assert.AreNotEqual(db.getSession(s), "");
        }

    }
}
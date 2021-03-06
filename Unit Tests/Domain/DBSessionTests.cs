﻿
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

        [TestMethod()]
        public void addSessionTest()
        {
            DBSession db = DBSession.getInstance();
            db.initSession();
            Session s = new Session();
            Assert.AreEqual(db.getSession(s), "");
                 
        }

        [TestMethod()]
        public void removeSessionTest()
        {
            DBSession db = DBSession.getInstance();
            db.initSession();
            Session s = new Session();
            db.removeSession(s);
            Assert.AreNotEqual(db.getSession(s), "");
        }

    }
}
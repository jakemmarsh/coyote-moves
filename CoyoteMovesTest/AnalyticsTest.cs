using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Data_Access;
using System.Collections.ObjectModel;
using CoyoteMoves.Models.RequestItems;

namespace CoyoteMovesTest
{
    [TestClass]
    public class AnalyticsTest
    {


        private AnalyticsDB _analyticsDB;

        [TestInitialize]
        public void setup()
        {
            _analyticsDB = new AnalyticsDB();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GetDataFromBeginningSuccess()
        {
            Collection<RequestForm> testAnalysis = _analyticsDB.GetAllApprovedRequests();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GetGroupChangeSuccess()
        {
            string test = _analyticsDB.GetAllGroupChangeInformation();
            Assert.AreEqual(test, "Group Unknown changed by -2 people\nGroup T7 changed by 1 people\nGroup C13 changed by -1 people\nGroup T22 changed by 2 people\n");
        }
    }
}

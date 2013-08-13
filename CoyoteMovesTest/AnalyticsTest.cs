using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Data_Access;
using System.Collections.ObjectModel;
using CoyoteMoves.Models.RequestItems;
using System.Data.SqlTypes;
using System.Xml;

namespace CoyoteMovesTest
{
    [TestClass]
    public class AnalyticsTest
    {
        private RequestFormDB _requester;
        private AnalyticsDB _analyticsDB;

        [TestInitialize]
        public void setup()
        {
            _requester = new RequestFormDB();
            _analyticsDB = new AnalyticsDB();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GetApprovedSuccess()
        {
            bool test = true;
            Collection<RequestForm> testAnalysis = _analyticsDB.GetAllApprovedRequests();
            foreach (RequestForm entry in testAnalysis)
            {
                if (!(_requester.HRApproved(entry.UniqueId) && _requester.SDApproved(entry.UniqueId)))
                    test = false;
            }
            Assert.IsTrue(test);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GetApprovedBetweenDatesSuccess()
        {
            bool test = true;
            SqlDateTime date = new SqlDateTime(2013, 8, 4);
            Collection<RequestForm> testAnalysis = _analyticsDB.GetApprovedRequestsBetweenDates(date, SqlDateTime.MaxValue);
            foreach (RequestForm entry in testAnalysis)
            {
                if ((!(_requester.HRApproved(entry.UniqueId) && _requester.SDApproved(entry.UniqueId))) || (entry.UniqueId.ToString() == "F2F56AB3-1F2B-45BA-842C-88918FF6551A"))
                    test = false;
            }
            Assert.IsTrue(test);
        }
    }
}
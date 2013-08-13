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
            Collection<RequestForm> testAnalysis = _analyticsDB.GetAllRequestRecords();
        }
    }
}

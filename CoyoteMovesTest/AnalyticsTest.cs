using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Data_Access;

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
            bool testAnalysis = _analyticsDB.GetAllRequestRecords();
            Assert.IsTrue(testAnalysis);
        }
    }
}

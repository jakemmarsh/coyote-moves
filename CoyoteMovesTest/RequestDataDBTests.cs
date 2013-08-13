using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Data_Access;
using System.Collections.Generic;

namespace CoyoteMovesTest
{
    [TestClass]
    public class RequestDataDBTests
    {
        private RequestDataDB _requester;

        [TestInitialize]
        public void setup()
        {
            _requester = new RequestDataDB();
        }

        [TestCleanup]
        public void cleanup()
        {
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GetAllDepartmentsSuccess()
        {
            List<string> realDeptList = new List<string>(){"Unknown", "Accounting", "Shipping", "Human Resources", "Executive Team", "Customer Sales", "Carrier Sales", 
                                            "Receiving", "Brokerage", "Administration",  "Intermodal Ops", "International", "Kenworth", "Pricing", "Sales", "IT", "Supply Chain", 
                                            "Compliance", "Driver Services"}; 
            List<string> deptList = new List<string>();
            deptList = _requester.GetAllDepartments();
            CollectionAssert.AreEqual(realDeptList, deptList);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GetAllGroupsSuccess()
        {

        }
    }
}

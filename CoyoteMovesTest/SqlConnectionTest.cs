using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves;
using CoyoteMoves.Data_Access;
using System.Collections.ObjectModel;
using CoyoteMoves.Models.EmployeeData;

namespace CoyoteMovesTest
{
    [TestClass]
    public class SqlConnectionTest
    {
        private IndividualRequestDB _requester = new IndividualRequestDB();
        private EmployeeDB _employee = new EmployeeDB();
        private DeskDB _desk = new DeskDB();


        [TestCategory("Integration")]
        [TestMethod]
        public void GetNameById()
        {
            string firstName = "Predrag";
            string lastName = "Djukic";
            int predragId = 47;
            
            string fullName = _requester.GetFullNameById(predragId);
 
            Assert.AreEqual(firstName + " " + lastName, fullName);
        }


        [TestCategory("Integration")]
        [TestMethod]
        public void GetPeopleByGroup()
        {
            Collection<int> expectedIds = new Collection<int> { 69, 540, 615, 896, 1425, 4092, 4253, 4684, 5266, 7944, 10113, 10179, 11832, 13493, 18016, 18020 };
            int testGroup = 13;

            Collection<int> retrievedIds = _requester.GetEmployeeIdsByGroupId(testGroup);

            for (int i = 0; i < expectedIds.Count; i++)
            {
                Assert.AreEqual(retrievedIds[i], expectedIds[i]);
            }
        }

        [TestCategory("Unit"), TestMethod]
        public void GetIdFromName()
        {
            int retrievedName = _employee.GetIdFromName("Jason Dibabbo");
            Assert.AreEqual(301758, retrievedName);
        }

    }
}

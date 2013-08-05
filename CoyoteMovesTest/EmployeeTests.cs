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
    public class EmployeeTests
    {
        private EmployeeDB _employee = new EmployeeDB();
        private DeskDB _desk = new DeskDB();


        [TestCategory("Integration")]
        [TestMethod]
        public void GetNameById()
        {
            string firstName = "Predrag";
            string lastName = "Djukic";
            int predragId = 47;

            string fullName = _employee.GetFullNameById(predragId);
 
            Assert.AreEqual(firstName + " " + lastName, fullName);
        }

        //refactor
        [TestCategory("Integration")]
        [TestMethod]
        public void GetPeopleByGroup()
        {
            Collection<int> expectedIds = new Collection<int> { 69, 540, 615, 896, 1425, 4092, 4253, 4684, 5266, 7944, 10113, 10179, 11832, 13493, 18016, 18020 };
            int testGroup = 13;

            Collection<int> retrievedIds = _employee.GetEmployeeIdsByGroupId(testGroup);

            for (int i = 0; i < expectedIds.Count; i++)
            {
                Assert.AreEqual(retrievedIds[i], expectedIds[i]);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetIdFromName()
        {
            int retrievedName = _employee.GetIdFromName("Jason Dibabbo");
            Assert.AreEqual(301758, retrievedName);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void GetIdFromNameFailsNoExist()
        {
            int retrievedName = _employee.GetIdFromName("Ivanna Humpalot");
            Assert.AreEqual(-1, retrievedName);

        }

        [TestCategory("Integration")]
        [TestMethod]
        public void GetNumberOfEmployeesInEachGroup()
        {
            Dictionary<string, int> dict = _employee.GetNumberOfEmployeesInEachGroup();
            int total = 0;
            foreach (KeyValuePair<string, int> pair in dict)
            {
                Console.WriteLine(pair.Key + " : " + pair.Value);
                total += pair.Value;
            }
            Console.WriteLine("Total : " + total);
        }
    }
}

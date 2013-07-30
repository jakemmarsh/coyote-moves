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
        IndividualRequestDB _requester = new IndividualRequestDB();

        [TestCategory("Integration")]
        [TestMethod]
        public void GetPredragNameById()
        {
            string firstName = "Predrag";
            string lastName = "Djukic";
            int predragId = 47;
            
            string fullName = _requester.GetFullNameById(predragId);
 
            Assert.AreEqual(firstName + " " + lastName, fullName);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void GetEmployeeObjectById()
        {

            string firstName = "Kevin";
            string lastName = "Stacy-Blake";
            string jobTitle = "17";
            string email = "kevin.stacyblake@coyote.com";
            string department = "5";
            string group = "6";
            string managerName = "Bobby Bruno";  //get by Manager ID
            //string template;
            //string securityItemRights;

            int kevinId = 299364;
            Employee testEmployee = new Employee();

            testEmployee = _requester.GetEmployeeById(kevinId);

            Assert.AreEqual(kevinId, testEmployee.Id);
            Assert.AreEqual(firstName, testEmployee.FirstName);
            Assert.AreEqual(lastName, testEmployee.LastName);
            Assert.AreEqual(email, testEmployee.Email);
            Assert.AreEqual(jobTitle, testEmployee.JobTitle);
            Assert.AreEqual(department, testEmployee.Department);
            Assert.AreEqual(group, testEmployee.Group);
            Assert.AreEqual(managerName, testEmployee.ManagerName);
            //Assert.AreEqual(template, testEmployee.Template);
            //Assert.AreEqual(securityItemRights, testEmployee.SecurityItemRights);
               
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
            EmployeeDB empDb = new EmployeeDB();
            Assert.AreEqual(301758, empDb.GetIdFromName("Jason DiBabbo"));
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Data_Access;
using CoyoteMoves.Models.SeatingData;
using System.Collections.Generic;

namespace CoyoteMovesTest
{
    [TestClass]
    public class DeskTests
    {

        private InfoValidator _validator;
        private DeskDB _testDesk;
        private string _deskNum;
        private int _topLeftX, _topLeftY, _orient, _floorNum, _empID;

        [TestInitialize]
        public void setup()
        {
            _deskNum = "THX1138";
            _topLeftX = 0;
            _topLeftY = 1;
            _orient = 0;
            _floorNum = 5;
            _empID = 117;
            _testDesk = new DeskDB();
            _validator = new InfoValidator();
        }

        //[TestMethod]
        //[TestCategory("Integration")]
        //public void TestAddNamesAndDeskNumbersFromFile()
        //{
        //    DeskDB test = new DeskDB();
        //    //this function takes three minutes to run...

        //    //Do NOT run this function, unless you want to insert ~1000 people into the database
        //    //test.AddNamesAndDeskNumbersFromFile(@"C:\\Users\mitchell.hymel\Downloads\Blueprints.txt");
        //}

        [TestMethod]
        [TestCategory("Integration")]
        public void DeskInsertedSuccessfully()
        {
            bool validation1 = _testDesk.InsertInformationIntoDeskDB(_deskNum, _topLeftX, _topLeftY, _orient, _floorNum, _empID);
            bool validation2 = _validator.ValidateDeskNumber(_deskNum);
            Assert.IsTrue(validation1);
            Assert.IsTrue(validation2);
            bool removeValidation = _testDesk.RemoveDeskFromDeskDB(_deskNum);
            Assert.IsTrue(removeValidation);
            
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(System.Data.SqlClient.SqlException))]
        public void DeskInsertFailedNoEmployeeExists()
        {
            int _notAPerson = 118;
            bool validation = _testDesk.InsertInformationIntoDeskDB(_deskNum, _topLeftX, _topLeftY, _orient, _floorNum, _notAPerson);
            Assert.IsFalse(validation);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GetAllDesksByFloorSuccessful()
        {
            List<Desk> testList = new List<Desk>();
            testList = _testDesk.GetDesksFromFloor(5);
            foreach (Desk entry in testList)
            {
                bool validation = _validator.ValidateDeskNumber(entry.DeskNumber);
                Assert.IsTrue(validation);
                int number = entry.Location.Floor;
                Assert.AreEqual(5, number);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public void CheckIfDeskExists()
        {
            Assert.IsTrue(_testDesk.CheckIfDeskExisits("5-1"));
            Assert.IsFalse(_testDesk.CheckIfDeskExisits("5-888"));
        }
    }
}

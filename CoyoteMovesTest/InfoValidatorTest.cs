using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Models.RequestItems;
using CoyoteMoves.Models.RequestItems.RequestTypes;
using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Data_Access;
using System.Collections.Generic;

namespace CoyoteMovesTest
{
    [TestClass]
    public class InfoValidatorTest
    {
        InfoValidator _validator;

        [TestInitialize]
        public void setup()
        {
           _validator = new InfoValidator();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestValidateDeskNumber()
        {
            Assert.IsTrue(_validator.ValidateDeskNumber("5-1"));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestValidateDeskNumberFailsNull()
        {
            Assert.IsFalse(_validator.ValidateDeskNumber("IMNOTINTHEDATABASE"));
        }
    }
}

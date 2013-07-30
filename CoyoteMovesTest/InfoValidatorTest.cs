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
        [TestMethod]
        public void TestValidateDeskNumber()
        {
            InfoValidator validator = new InfoValidator();

            Assert.IsTrue(validator.ValidateDeskNumber("ABC666"));
            Assert.IsFalse(validator.ValidateDeskNumber("IMNOTINTHEDATABASE"));
        }
    }
}

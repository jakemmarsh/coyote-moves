using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves;
using CoyoteMoves.Data_Access;

namespace CoyoteMovesTest
{
    [TestClass]
    public class SqlConnectionTest
    {
        IndividualRequest _requester = new IndividualRequest();

        [TestCategory ("Integration")]
        [TestMethod]
        public void testTest_testDotTest()
        {    
            int pleasebe46;
            pleasebe46 = _requester.GenericIntCall("SELECT TOP 1 [EmployeeID] FROM [Intern_CoyoteMoves].[dbo].[InternalEmployee]");
            
            Assert.AreEqual(46, pleasebe46);
        }
    }
}

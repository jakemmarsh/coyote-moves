using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Data_Access;

namespace CoyoteMovesTest
{
    [TestClass]
    public class InitializeDeskDBTest
    {
        [TestMethod]
        public void TestAddNamesAndDeskNumbersFromFile()
        {
            InitializeDeskDB test = new InitializeDeskDB();
            //this function takes three minutes to run...

            //Do NOT run this function, unless you want to insert ~1000 people into the database
            //test.AddNamesAndDeskNumbersFromFile(@"C:\\Users\mitchell.hymel\Downloads\Blueprints.txt");
        }
    }
}

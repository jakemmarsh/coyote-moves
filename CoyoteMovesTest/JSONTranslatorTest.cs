using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Controllers;
using CoyoteMoves.Models.RequestItems;
using CoyoteMoves.Models.SeatingData;
using System.Collections.Generic;

namespace CoyoteMovesTest
{
    [TestClass]
    public class JSONTranslatorTest
    {
        [TestMethod]
        public void JSONTest()
        {
            JSONTranslator parser = new JSONTranslator();

            Desk myDesk = new Desk(3, "First Desk");
            Desk desktwo = new Desk(4, "Second Desk");

            List<Desk> deskcollection = new List<Desk>();

            deskcollection.Add(myDesk);
            deskcollection.Add(desktwo);

            Floor floor = new Floor(1, deskcollection);
            string result = parser.GetJSONFromFloor(floor);

            Console.WriteLine(result);
        }
    }
}
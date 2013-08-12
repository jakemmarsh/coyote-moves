using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Controllers;
using System.Net.Http;

namespace CoyoteMovesTest
{
    [TestClass]
    public class ControllerTests
    {
        RequestFormController _controller;

        [TestInitialize]
        public void setup()
        {
           _controller = new RequestFormController();
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void TestMethod1()
        {
          //Just need to unit test
            
        }
    }
}

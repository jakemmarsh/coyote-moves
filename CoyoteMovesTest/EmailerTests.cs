using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Emailer;
using CoyoteMoves.Emailer.Models;


namespace CoyoteMovesTest
{
    [TestClass]
    public class EmailerTests
    {
        [TestCategory ("Unit")]
        [TestMethod]
        public void firstPDFTest()
        {
            var _mailBody = new MailBody();
            _mailBody.fillOutPdfYolo();

            Assert.IsNotNull(_mailBody);
        }
           
    }
}

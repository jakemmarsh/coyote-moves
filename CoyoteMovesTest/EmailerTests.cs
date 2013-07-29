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

        [TestCategory("Unit")]
        [TestMethod]
        public void apocalypticEmailTest()
        {
            var _mailBody = new MailBody();

            bool mailTest = _mailBody.sendTestEmail("jake.marsh@coyote.com");
            bool otherMailTest = _mailBody.sendTestEmail("kevin.jasieniecki@coyote.com");

            Assert.IsTrue(mailTest);
            Assert.IsTrue(otherMailTest);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void sendEmailAttachment()
        {
            var _mailBody = new MailBody();

         
            bool testMail = _mailBody.sendTestAttachment("coyoteinterns2013@googlegroups.com");
            bool otherMailTest = _mailBody.sendTestAttachment("kevin.jasieniecki@coyote.com");

            Assert.IsTrue(otherMailTest);
            Assert.IsTrue(testMail);
        }
           
    }

}

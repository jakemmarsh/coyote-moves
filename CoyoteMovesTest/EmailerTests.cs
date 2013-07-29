using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Emailer;
using CoyoteMoves.Emailer.Models;
using CoyoteMoves.Models.RequestItems;
using CoyoteMoves.Models.RequestItems.RequestTypes;
using System.Collections.Generic;


namespace CoyoteMovesTest
{
    [TestClass]
    public class EmailerTests
    {
        [TestCategory ("Unit")]
        [TestMethod]
        public void gimmeDaFields()
        {
            var _EmailSender = new EmailSender();
            _EmailSender.getFieldNames("../../../CoyoteMoves/CoyoteMovesTemplate.pdf");

            Assert.IsNotNull(_EmailSender);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void apocalypticEmailTest()
        {
            var _EmailSender = new EmailSender();

            bool mailTest = _EmailSender.sendTestEmail("jake.marsh@coyote.com");
            bool otherMailTest = _EmailSender.sendTestEmail("kevin.jasieniecki@coyote.com");

            Assert.IsTrue(mailTest);
            Assert.IsTrue(otherMailTest);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void sendEmailAttachment()
        {
            var _EmailSender = new EmailSender();
            bool otherMailTest = _EmailSender.sendTestAttachment("kevin.jasieniecki@coyote.com");
            Assert.IsTrue(otherMailTest);
      
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void celebrationBraggingTest()
        {
            var _EmailSender = new EmailSender();
            bool mailMeBro = _EmailSender.sendTestAttachment("ian.lovrich@coyote.com");
            bool mailmetoo = _EmailSender.sendTestAttachment("jason.dibabbo@coyote.com");
            bool mailmethree = _EmailSender.sendTestAttachment("brandon.dsouza@coyote.com");
            bool mailmefour = _EmailSender.sendTestAttachment("mitchell.hymel@coyote.com");
            bool mailmefive = _EmailSender.sendTestAttachment("stephen.wan@coyote.com");
            bool fukthapolice = _EmailSender.sendTestAttachment("kevin.jasieniecki@coyote.com");


            Assert.IsTrue(mailMeBro);
            Assert.IsTrue(mailmetoo);
            Assert.IsTrue(mailmethree);
            Assert.IsTrue(mailmefour);
            Assert.IsTrue(mailmefive);
            Assert.IsTrue(fukthapolice);

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void emailFromRequestObject()
        {
            RequestForm req = new RequestForm();
            req.Current = new CoyoteMovesFormEmployeeInfo();
            req.Future = new CoyoteMovesFormEmployeeInfo();
            req.Current.BazookaInfo = new BazookaInfo();
            req.Future.BazookaInfo = new BazookaInfo();
            req.Current.UltiproInfo = new UltiproInfo();
            req.Future.UltiproInfo = new UltiproInfo();
            req.Current.DeskInfo = new DeskInfo();
            req.Future.DeskInfo = new DeskInfo();
            req.Current.PhoneInfo = new PhoneInfo();
            req.Future.PhoneInfo = new PhoneInfo();
            req.EmailInfo = new EmailDistributionInfo();
            req.ReviewInfo = new EmployeeReviewFileInfo();

            EmailSender _emailer = new EmailSender();

            req.Current.BazookaInfo.JobTitle = "Fisherman";
            req.Future.BazookaInfo.JobTitle = "Whaler";
            req.Current.BazookaInfo.Department = "Raynor's Raiders";
            req.Future.BazookaInfo.Department = "THE SWARM";
            req.Current.BazookaInfo.Group = "The Backstreet Boys";
            req.Future.BazookaInfo.Group = "One Direction";
            req.Current.BazookaInfo.Manager = "Bob from Accounting";
            req.Future.BazookaInfo.Manager = "Korlash, Lord of Lies";
            req.Current.BazookaInfo.JobTemplate = "wtf";
            req.Future.BazookaInfo.JobTemplate = "srsly";
            req.Current.BazookaInfo.SecurityItemRights = "Snowden-level";
            req.Future.BazookaInfo.SecurityItemRights = "sudo apt-get dicks";

            req.Current.UltiproInfo.JobTitle = "Fisherman Apprentice";
            req.Future.UltiproInfo.JobTitle = "Ahab";
            req.Current.UltiproInfo.Department = "Mergers and Acquisitions";
            req.Future.UltiproInfo.Department = "Murders and Assassinations";
            req.Current.UltiproInfo.Supervisor = "Draco Malfoy";
            req.Future.UltiproInfo.Supervisor = "Harry Potter"; 
            req.Current.UltiproInfo.Other = "Option for Otherkin employees";
            req.Future.UltiproInfo.Other = "Should this be nullabe?";
           
            req.Current.DeskInfo.DeskNumber = "666";
            req.Future.DeskInfo.DeskNumber = "616";
            req.Current.DeskInfo.Office = "The 6th Circle";
            req.Future.DeskInfo.Office = "The 7th Gate";

            req.Current.PhoneInfo.PhoneNumber = "8472718339";
            req.Future.PhoneInfo.PhoneNumber = "99995953214";

            req.EmailInfo.GroupsToBeAddedTo = new List<string> { "wat", "the", "fak" };
            req.EmailInfo.GroupsToBeRemovedFrom = new List<string> { "are", "you", "doing" };

            req.ReviewInfo.FilesToBeAddedTo = new List<string> { "the", "files", "are" };
            req.ReviewInfo.FilesToBeRemovedFrom = new List<string> { "in", "the", "computer" };

            bool testSent = _emailer.sendMovesRequest(req, "kevin.jasieniecki@coyote.com");
            bool mailMeBro = _emailer.sendMovesRequest(req,"ian.lovrich@coyote.com");
            bool mailmetoo = _emailer.sendMovesRequest(req,"jason.dibabbo@coyote.com");
            bool mailmethree =_emailer.sendMovesRequest(req,"brandon.dsouza@coyote.com");
            bool mailmefour = _emailer.sendMovesRequest(req,"mitchell.hymel@coyote.com");
            bool mailmefive = _emailer.sendMovesRequest(req,"stephen.wan@coyote.com");


            Assert.IsTrue(mailMeBro);
            Assert.IsTrue(mailmetoo);
            Assert.IsTrue(mailmethree);
            Assert.IsTrue(mailmefour);
            Assert.IsTrue(mailmefive);

            Assert.IsTrue(testSent);
            

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void readFieldsWhatTheFuck()
        {

            EmailSender _emailSender = new EmailSender();
            _emailSender.getFieldNames("../../../CoyoteMoves/testdoc.pdf");

            Assert.IsTrue(true);

        }
           
    }

}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Emailer.Models;
using CoyoteMoves.Models.RequestItems;
using CoyoteMoves.Models.RequestItems.RequestTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;


namespace CoyoteMovesTest
{
    [TestClass]
    public class EmailerTests
    {
        private EmailSender _emailer;
        private RequestForm _req;
        private Collection<string> _to;

        [TestInitialize]
        public void Setup()
        {
            _to = new Collection<string> {};
            _emailer = new EmailSender("New Coyote Moves _request", _to, "CoyoteMoves_request@coyote.com", "Here you go!", "../../../CoyoteMoves/CoyoteMovesTemplate.pdf");
            _req = new RequestForm();


            _req.Current = new CoyoteMovesFormEmployeeInfo();
            _req.Future = new CoyoteMovesFormEmployeeInfo();
            _req.Current.BazookaInfo = new BazookaInfo();
            _req.Future.BazookaInfo = new BazookaInfo();
            _req.Current.UltiproInfo = new UltiproInfo();
            _req.Future.UltiproInfo = new UltiproInfo();
            _req.Current.DeskInfo = new DeskInfo();
            _req.Future.DeskInfo = new DeskInfo();
            _req.Current.PhoneInfo = new PhoneInfo();
            _req.Future.PhoneInfo = new PhoneInfo();
            _req.EmailInfo = new EmailDistributionInfo();
            _req.ReviewInfo = new EmployeeReviewFileInfo();


            _req.Current.BazookaInfo.JobTitle = "test";
            _req.Future.BazookaInfo.JobTitle = "test";
            _req.Current.BazookaInfo.Department = "test";
            _req.Future.BazookaInfo.Department = "test";
            _req.Current.BazookaInfo.Group = "test";
            _req.Future.BazookaInfo.Group = "test";
            _req.Current.BazookaInfo.ManagerID = 49;
            _req.Future.BazookaInfo.ManagerID = 50;
            _req.Current.BazookaInfo.JobTemplate = "test";
            _req.Future.BazookaInfo.JobTemplate = "testy";
            _req.Current.BazookaInfo.SecurityItemRights = "test";
            _req.Future.BazookaInfo.SecurityItemRights = "test";

            _req.Current.UltiproInfo.JobTitle = "test";
            _req.Future.UltiproInfo.JobTitle = "test";
            _req.Current.UltiproInfo.Department = "test";
            _req.Future.UltiproInfo.Department = "test";
            _req.Current.UltiproInfo.Supervisor = "test";
            _req.Future.UltiproInfo.Supervisor = "test"; 
            _req.Current.UltiproInfo.Other = "test";
            _req.Future.UltiproInfo.Other = "test?";
           
            _req.Current.DeskInfo.DeskNumber = "test";
            _req.Future.DeskInfo.DeskNumber = "test";
            _req.Current.DeskInfo.Office = "test";
            _req.Future.DeskInfo.Office = "test";

            _req.Current.PhoneInfo.PhoneNumber = "8472718339";
            _req.Future.PhoneInfo.PhoneNumber = "99995953214";

            _req.EmailInfo.GroupsToBeAddedTo = new List<string> { "one", "two", "three" };
            _req.EmailInfo.GroupsToBeRemovedFrom = new List<string> { "one", "two", "three" };

            _req.ReviewInfo.FilesToBeAddedTo = new List<string> { "one", "two", "three" };
            _req.ReviewInfo.FilesToBeRemovedFrom = new List<string> { "one", "two", "three" };

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void emailFromRequestObjectIsSent()
        {
            _to.Add("kevin.jasieniecki@coyote.com");
            bool testSent = _emailer.sendMovesRequest(_req);

            Assert.IsTrue(testSent);
        }

        [TestCategory("Unit")]
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void emailFailsIfNoRecipients()
        {
            _to.Add(null);
            bool testSent = _emailer.sendMovesRequest(_req);
        }

        [TestCategory("Unit")]
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void emailFailsIfNoRequest()
        {
            bool testSent = _emailer.sendMovesRequest(null);
        }

        [TestCategory("Unit")]
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void HRemailFailsIfNoRequest()
        {
            bool testSent = _emailer.sendMovesRequestHR(null);
        }

        [TestCategory("Unit")]
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void SDemailFailsIfNoRequest()
        {
            bool testSent = _emailer.sendMovesRequestSD(null);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void nonHRorSDEmailFails()
        {
            bool testSent = _emailer.sendMovesRequest(_req, "GX");
            Assert.IsFalse(testSent);
        }

      

    }


}

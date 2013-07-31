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
            Collection<string> to = new Collection<string> { "kevin.jasieniecki@coyote.com" };
            _emailer = new EmailSender("New Coyote Moves _request", to, "CoyoteMoves_request@coyote.com", "Here you go!", "../../../CoyoteMoves/CoyoteMovesTemplate.pdf");
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


            _req.Current.BazookaInfo.JobTitle = "Fisherman";
            _req.Future.BazookaInfo.JobTitle = "Whaler";
            _req.Current.BazookaInfo.Department = "Raynors Raiders";
            _req.Future.BazookaInfo.Department = "THE SWARM";
            _req.Current.BazookaInfo.Group = "The Backstreet Boys";
            _req.Future.BazookaInfo.Group = "One Direction";
            _req.Current.BazookaInfo.ManagerID = 49;
            _req.Future.BazookaInfo.ManagerID = 50;
            _req.Current.BazookaInfo.JobTemplate = "wtf";
            _req.Future.BazookaInfo.JobTemplate = "srsly";
            _req.Current.BazookaInfo.SecurityItemRights = "Snowden-level";
            _req.Future.BazookaInfo.SecurityItemRights = "sudo apt-get dicks";

            _req.Current.UltiproInfo.JobTitle = "Fisherman Apprentice";
            _req.Future.UltiproInfo.JobTitle = "Ahab";
            _req.Current.UltiproInfo.Department = "Mergers and Acquisitions";
            _req.Future.UltiproInfo.Department = "Murders and Assassinations";
            _req.Current.UltiproInfo.Supervisor = "Draco Malfoy";
            _req.Future.UltiproInfo.Supervisor = "Harry Potter"; 
            _req.Current.UltiproInfo.Other = "Option for Otherkin employees";
            _req.Future.UltiproInfo.Other = "Should this be nullabe?";
           
            _req.Current.DeskInfo.DeskNumber = "666";
            _req.Future.DeskInfo.DeskNumber = "616";
            _req.Current.DeskInfo.Office = "The 6th Circle";
            _req.Future.DeskInfo.Office = "The 7th Gate";

            _req.Current.PhoneInfo.PhoneNumber = "8472718339";
            _req.Future.PhoneInfo.PhoneNumber = "99995953214";

            _req.EmailInfo.GroupsToBeAddedTo = new List<string> { "wat", "the", "fak" };
            _req.EmailInfo.GroupsToBeRemovedFrom = new List<string> { "are", "you", "doing" };

            _req.ReviewInfo.FilesToBeAddedTo = new List<string> { "the", "files", "are" };
            _req.ReviewInfo.FilesToBeRemovedFrom = new List<string> { "in", "the", "computer" };

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void emailFrom_requestObjectIsSent()
        {
            bool testSent = _emailer.sendMovesRequest(_req);

            Assert.IsTrue(testSent);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void emailStoredTest()
        {
            bool testStore = _emailer.sendMovesRequestAndStore(_req);

            Assert.IsTrue(testStore);


        }
    }

}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Emailer;
using CoyoteMoves.Emailer.Models;
using CoyoteMoves.Models.RequestItems;
using CoyoteMoves.Models.RequestItems.RequestTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace CoyoteMovesTest
{
    [TestClass]
    public class EmailerTests
    {

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

            Collection<string> to = new Collection<string> {"kevin.jasieniecki@coyote.com"};
            EmailSender _emailer = new EmailSender("New Coyote Moves Request", to, "CoyoteMovesRequest@coyote.com", "Here you go!", "../../../CoyoteMoves/CoyoteMovesTemplate.pdf");

            req.Current.BazookaInfo.JobTitle = "Fisherman";
            req.Future.BazookaInfo.JobTitle = "Whaler";
            req.Current.BazookaInfo.Department = "Raynor's Raiders";
            req.Future.BazookaInfo.Department = "THE SWARM";
            req.Current.BazookaInfo.Group = "The Backstreet Boys";
            req.Future.BazookaInfo.Group = "One Direction";
            req.Current.BazookaInfo.ManagerID = 49;
            req.Future.BazookaInfo.ManagerID = 50;
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

            bool testSent = _emailer.sendMovesRequest(req);

            Assert.IsTrue(testSent);
        }
    }

}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Models.RequestItems;
using CoyoteMoves.Models.RequestItems.RequestTypes;
using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Data_Access;
using System.Collections.Generic;

namespace CoyoteMovesTest
{
    [TestClass]
    public class RequestFormDBTest
    {
        private RequestForm _req;
        private RequestFormDB _requester;
        private InfoValidator _validator;
        
        [TestInitialize]
        public void setup()
        {
            _requester = new RequestFormDB();
            _validator = new InfoValidator();
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void TestStoreRequestFormInDatabaseAsPending()
        {
            string test = "test";
            _req = new RequestForm(301757);

            _req.Current.BazookaInfo.JobTitle = "Intern";
            _req.Current.BazookaInfo.JobTemplate = test;
            _req.Current.BazookaInfo.ManagerID = 301757;
            _req.Current.BazookaInfo.Group = "T1";
            _req.Current.BazookaInfo.SecurityItemRights = test;
            _req.Current.BazookaInfo.Department = "IT";
            _req.Future.BazookaInfo.JobTitle = "Intern";
            _req.Future.BazookaInfo.JobTemplate = test;
            _req.Future.BazookaInfo.ManagerID = 301757;
            _req.Future.BazookaInfo.Group = "T1";
            _req.Future.BazookaInfo.SecurityItemRights = test;
            _req.Future.BazookaInfo.Department = "IT";

            _req.Current.DeskInfo.DeskNumber = test;
            _req.Current.DeskInfo.Office = test;
            _req.Future.DeskInfo.DeskNumber = test;
            _req.Future.DeskInfo.Office = test;

            _req.Current.PhoneInfo.PhoneNumber = test;
            _req.Future.PhoneInfo.PhoneNumber = test;

            _req.Current.UltiproInfo.Department = test;
            _req.Current.UltiproInfo.JobTitle = test;
            _req.Current.UltiproInfo.Other = test;
            _req.Current.UltiproInfo.Supervisor = test;
            _req.Future.UltiproInfo.Department = test;
            _req.Future.UltiproInfo.JobTitle = test;
            _req.Future.UltiproInfo.Other = test;
            _req.Future.UltiproInfo.Supervisor = test;

            _requester.StoreRequestFormInDatabaseAsPending(_req);



        }

        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateServiceDeskApprovedStatus()
        {
            bool requestValidation = _requester.UpdateRequestToServiceDeskApproved(1);
            bool testValidation = _validator.ValidateServiceDeskApproval(1);
            Assert.IsTrue(testValidation);
            Assert.IsTrue(requestValidation);
        }
        
        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateHumanResourcesApprovedStatus()
        {
            bool requestValidation = _requester.UpdateRequestToHRApproved(1);
            bool testValidation = _validator.ValidateHumanResourcesApproval(1);
            Assert.IsTrue(testValidation);
            Assert.IsTrue(requestValidation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateHRApprovedFailedNoRequestFound()
        {
            bool requestValidation = _requester.UpdateRequestToHRApproved(0);
            Assert.IsFalse(requestValidation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateSDApprovedFailedNoRequestFound()
        {
            bool requestValidation = _requester.UpdateRequestToServiceDeskApproved(0);
            Assert.IsFalse(requestValidation);
        }


    }
}

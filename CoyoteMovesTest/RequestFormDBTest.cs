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
            RequestForm form = new RequestForm(301757);
            form.EmployeeId = 301757;

            form.Current.BazookaInfo.JobTitle = "Intern";
            form.Current.BazookaInfo.JobTemplate = test;
            form.Current.BazookaInfo.ManagerID = 301757;
            form.Current.BazookaInfo.Group = "T1";
            form.Current.BazookaInfo.SecurityItemRights = test;
            form.Current.BazookaInfo.Department = "IT";
            form.Future.BazookaInfo.JobTitle = "Intern";
            form.Future.BazookaInfo.JobTemplate = test;
            form.Future.BazookaInfo.ManagerID = 301757;
            form.Future.BazookaInfo.Group = "T1";
            form.Future.BazookaInfo.SecurityItemRights = test;
            form.Future.BazookaInfo.Department = "IT";

            form.Current.DeskInfo.DeskNumber = test;
            form.Current.DeskInfo.Office = test;
            form.Future.DeskInfo.DeskNumber = test;
            form.Future.DeskInfo.Office = test;

            form.Current.PhoneInfo.PhoneNumber = test;
            form.Future.PhoneInfo.PhoneNumber = test;

            form.Current.UltiproInfo.Department = test;
            form.Current.UltiproInfo.JobTitle = test;
            form.Current.UltiproInfo.Other = test;
            form.Current.UltiproInfo.Supervisor = test;
            form.Future.UltiproInfo.Department = test;
            form.Future.UltiproInfo.JobTitle = test;
            form.Future.UltiproInfo.Other = test;
            form.Future.UltiproInfo.Supervisor = test;

            RequestFormDB tester = new RequestFormDB();
            tester.StoreRequestFormInDatabaseAsPending(form);

            //now check the database for this addition
            //...for now, just go look manually...


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

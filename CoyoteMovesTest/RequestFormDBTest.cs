using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Models.RequestItems;
using CoyoteMoves.Models.RequestItems.RequestTypes;
using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Data_Access;
using System.Collections.Generic;
using CoyoteMoves.Models;
using System.Data.SqlClient;
using System.Reflection;

namespace CoyoteMovesTest
{
    [TestClass]
    public class RequestFormDBTest
    {
        private RequestForm _req;
        private RequestFormDB _requester;
        
        [TestInitialize]
        public void setup()
        {
            _requester = new RequestFormDB();
            _req = new RequestForm(301757);
            TestStoreRequestFormInDatabaseAsPending();        
        }

        [TestCleanup]
        public void cleanup()
        {
            _requester.RemoveRequestByUniqueId(_req.UniqueId);
        }

        public void TestStoreRequestFormInDatabaseAsPending()
        {
            string test = "test";

            _req.CreatedByID = 301758;
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

            _req.Current.UltiproInfo.Department = "IT";
            _req.Current.UltiproInfo.JobTitle = "Intern";
            _req.Current.UltiproInfo.Other = test;
            _req.Current.UltiproInfo.Supervisor = "Mitchell Hymel";
            _req.Future.UltiproInfo.Department = "IT";
            _req.Future.UltiproInfo.JobTitle = "Intern";
            _req.Future.UltiproInfo.Other = test;
            _req.Future.UltiproInfo.Supervisor = "Mitchell Hymel";

            _requester.StoreRequestFormInDatabaseAsPending(_req);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateServiceDeskApprovedStatus()
        {
            bool requestValidation = _requester.UpdateRequestToServiceDeskApproved(_req.UniqueId);
            bool testValidation =_requester.SDApproved(_req.UniqueId);
            Assert.IsTrue(testValidation);
            Assert.IsTrue(requestValidation);
        }
        
        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateHumanResourcesApprovedStatus()
        {
            bool requestValidation = _requester.UpdateRequestToHRApproved(_req.UniqueId);
            bool testValidation = _requester.HRApproved(_req.UniqueId);
            Assert.IsTrue(testValidation);
            Assert.IsTrue(requestValidation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateHRApprovedFailedNoRequestFound()
        {
            Guid different = new Guid();
            bool requestValidation = _requester.UpdateRequestToHRApproved(different);
            Assert.IsFalse(requestValidation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateSDApprovedFailedNoRequestFound()
        {
            Guid different = new Guid();
            bool requestValidation = _requester.UpdateRequestToServiceDeskApproved(different);
            Assert.IsFalse(requestValidation);
        }

        [TestCategory("Integration"), TestMethod]
        public void TestSettingRequestToHRApproved()
        {
            Assert.IsTrue(_requester.UpdateRequestToHRApproved(_req.UniqueId));
            Assert.IsTrue(_requester.HRApproved(_req.UniqueId));
            Assert.IsFalse(_requester.SDApproved(_req.UniqueId));
            Assert.IsTrue(_requester.UpdateRequestToServiceDeskApproved(_req.UniqueId));
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void RequestRetrievedSuccessfully()
        {
            RequestForm testRequest = new RequestForm();
            testRequest = _requester.RetrieveRequest(_req.UniqueId);
            bool test = CheckEquality(_req, testRequest);
            Assert.IsTrue(test);
           
        }


        public bool CheckEquality(object source, object target)
        {
            if (source == null || target == null)
                return false;
            bool test = true;
            PropertyInfo[] sourceProperties = source.GetType().GetProperties();
            foreach (PropertyInfo sourcePropertyInfo in sourceProperties)
            {
                PropertyInfo targetPropertyInfo = target.GetType().GetProperty(sourcePropertyInfo.Name);
                if (!(targetPropertyInfo == sourcePropertyInfo))
                    test = false;

            }
            return test;

        }
    }
}

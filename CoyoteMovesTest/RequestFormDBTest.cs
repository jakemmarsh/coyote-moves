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
        [TestMethod]
        public void TestStoreRequestFormInDatabaseAsPending()
        {
            string test = "test";
            //make the object
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


            //call the function
            RequestFormDB tester = new RequestFormDB();
            tester.StoreRequestFormInDatabaseAsPending(form);

            //now check the database for this addition
            //...for now, just go look manually...


        }

        [TestMethod]
        public void TestUpdateRequestToApprovedStatus()
        {
            RequestFormDB tester = new RequestFormDB();
            //tester.UpdateRequestToHRApproved(1);
            tester.UpdateRequestToServiceDeskApproved(1);

            //now check the database for the update
            //...for now, just go look manually...
        }
    }
}

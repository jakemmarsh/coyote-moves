using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoyoteMoves.Models.RequestItems;
using System.Data.SqlClient;

namespace CoyoteMoves.Data_Access
{
    public class RequestFormDB
    {
        public string _connectionString { get; set; }

        public RequestFormDB()
        {
            _connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }

        /// <summary>
        /// Log the request in the database. Only mark it as pending until it is approved
        /// </summary>
        public bool StoreRequestFormInDatabaseAsPending(RequestForm form)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            //get the sql command with all the parameters
            SqlCommand command = AddParametersForStoreRequestFormInDatabaseHelper(form);

            command.Connection = connection;

            //probably wrap a try/catch around this...
            command.Connection.Open();
            int result = command.ExecuteNonQuery();
            command.Connection.Close();

            return (result == 1);
            
        }

        /// <summary>
        /// Once the request has been approved by HR and service desk, then we need to mark that in the database
        /// Given the requestid, find the request and change the pending to false and approved to true
        /// Do we have to keep a date of when it was approved(i.e. when the last of HR or service desk approved)? when it was marked as approve in the database?
        /// </summary>
        private bool UpdateRequestToApprovedStatus(int RequestID, string ApprovalDept)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            //probably add some check to make sure both Service desk and HR approved
            SqlCommand command = new SqlCommand("UPDATE Intern_CoyoteMoves.dbo.RequestData SET " + ApprovalDept + "Approved='1' WHERE RequestID="+ RequestID);
            command.Connection = connection;
            command.Connection.Open();

            int result = command.ExecuteNonQuery();
            command.Connection.Close();

            bool theOtherDepartmentHasApproved = CheckOtherDepartmentApproval(RequestID, ApprovalDept);
            if (theOtherDepartmentHasApproved)
            {
                setTheRequestAsNotPending(RequestID);
            }

            return (result == 1);

        }

        private void setTheRequestAsNotPending(int RequestID)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Intern_CoyoteMoves.dbo.RequestData SET Pending = 0 WHERE RequestID = " + RequestID);
            cmd.Connection = connection;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private bool CheckOtherDepartmentApproval(int RequestID, string ApprovalDept)
        {
            string OtherDept = ApprovalDept.Equals("HR") ? "ServiceDesk" : "HR";
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("SELECT " + OtherDept + "Approved FROM dbo.RequestData WHERE RequestID=" + RequestID);
            cmd.Connection = connection;
            cmd.Connection.Open();

            object temp = cmd.ExecuteScalar();
            connection.Close();
            return (bool)temp;
        }

        /// <summary>
        /// Since we have to add hella parameters to the stored proc, might as well create it's own function to do so
        /// </summary>
        /// <param name="commandString"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        private SqlCommand AddParametersForStoreRequestFormInDatabaseHelper(RequestForm form)
        {
            //USE COMMAND.PARAMETERS.ADDWITHVALUE NIGGA
            string commandString = "EXEC dbo.spRequestData_StoreRequestAsPending " +
                "@EmployeeID=" + form.EmployeeId + ", " +
                "@C_JobTitle='" + form.Current.BazookaInfo.JobTitle + "', " +
                "@C_Department='" + form.Current.BazookaInfo.Department + "', " +
                "@C_Group='" + form.Current.BazookaInfo.Group + "', " +
                "@C_ManagerID='" + form.Current.BazookaInfo.ManagerID + "', " +
                "@C_JobTemplate='" + form.Current.BazookaInfo.JobTemplate + "', " +
                "@C_SecurityItemRights='" + form.Current.BazookaInfo.SecurityItemRights + "', " +
                "@C_DeskNumber='" + form.Current.DeskInfo.DeskNumber + "', " +
                "@C_Office='" + form.Current.DeskInfo.Office + "', " +
                "@C_PhoneNumber='" + form.Current.PhoneInfo.PhoneNumber + "', " +
                "@C_Other='" + form.Current.UltiproInfo.Other + "', " +
                "@F_JobTitle='" + form.Future.BazookaInfo.JobTitle + "', " +
                "@F_Department='" + form.Future.BazookaInfo.Department + "', " +
                "@F_Group='" + form.Future.BazookaInfo.Group + "', " +
                "@F_ManagerID='" + form.Future.BazookaInfo.ManagerID + "', " +
                "@F_JobTemplate='" + form.Future.BazookaInfo.JobTemplate + "', " +
                "@F_SecurityItemRights='" + form.Future.BazookaInfo.SecurityItemRights + "', " +
                "@F_DeskNumber='" + form.Future.DeskInfo.DeskNumber + "', " +
                "@F_Office='" + form.Future.DeskInfo.Office + "', " +
                "@F_PhoneNumber='" + form.Future.PhoneInfo.PhoneNumber + "', " +
                "@F_Other='" + form.Future.UltiproInfo.Other + "', " +
                //need to change these...
                "@EmailListsToBeAddedTo='" + form.EmailInfo.GroupsToBeAddedTo.ToString() + "', " +
                "@EmailListsToBeRemovedFrom='" + form.EmailInfo.GroupsToBeRemovedFrom.ToString() + "', " +
                "@FilesToBeAddedTo='" + form.ReviewInfo.FilesToBeAddedTo.ToString() + "', " +
                "@FilesToBeRemovedFrom='" + form.ReviewInfo.FilesToBeRemovedFrom.ToString() + "', " + 
                "@CreateByID=" + 301758 + ", " + 
                "@UpdateByID=" + 301758;

            SqlCommand command = new SqlCommand(commandString);
            return command;
        }

        public bool UpdateRequestToServiceDeskApproved(int requestID)
        {
            return UpdateRequestToApprovedStatus(requestID, "ServiceDesk");
        }

        public bool UpdateRequestToHRApproved(int requestID)
        {
            return UpdateRequestToApprovedStatus(requestID, "HR");
        }
    }
}
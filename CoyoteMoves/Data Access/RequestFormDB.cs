using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoyoteMoves.Models.RequestItems;
using System.Data.SqlClient;
using System.Data;

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
        private bool UpdateRequestToApprovedStatus(Guid UniqueRequestID, string ApprovalDept)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            //probably add some check to make sure both Service desk and HR approved
            SqlCommand command = new SqlCommand("UPDATE Intern_CoyoteMoves.dbo.RequestData SET " + ApprovalDept + "Approved='1' WHERE UniqueRequestID="+ UniqueRequestID);
            command.Connection = connection;
            command.Connection.Open();

            int result = command.ExecuteNonQuery();
            command.Connection.Close();

            bool theOtherDepartmentHasApproved = CheckOtherDepartmentApproval(UniqueRequestID, ApprovalDept);
            if (theOtherDepartmentHasApproved)
            {
                setTheRequestAsNotPending(UniqueRequestID);
            }

            return (result == 1);

        }

        private void setTheRequestAsNotPending(Guid UniqueRequestID)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Intern_CoyoteMoves.dbo.RequestData SET Pending = 0 WHERE UniqueRequestID = " + UniqueRequestID);
            cmd.Connection = connection;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private bool CheckOtherDepartmentApproval(Guid UniqueRequestID, string ApprovalDept)
        {
            string OtherDept = ApprovalDept.Equals("HR") ? "ServiceDesk" : "HR";
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("SELECT " + OtherDept + "Approved FROM dbo.RequestData WHERE UniqueRequestID=" + UniqueRequestID);
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
            //USE COMMAND.PARAMETERS.ADDWITHVALUE YO
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
                "@UpdateByID=" + 301758 + ", " +
                "@UniqueID='" + form.UniqueId + "'";


=======
            string commandString = "[Intern_CoyoteMoves].[dbo].[spRequestData_StoreRequestAsPending]";
>>>>>>> master
            SqlCommand command = new SqlCommand(commandString);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            //for each parameter, add the parameter to the command and then set the parameter value
            command.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.Int));
            command.Parameters["@EmployeeID"].Value = form.EmployeeId;
            command.Parameters.Add(new SqlParameter("@C_JobTitle", SqlDbType.VarChar));
            command.Parameters["@C_JobTitle"].Value = form.Current.BazookaInfo.JobTitle;
            command.Parameters.Add(new SqlParameter("@C_Department", SqlDbType.VarChar));
            command.Parameters["@C_Department"].Value = form.Current.BazookaInfo.Department;
            command.Parameters.Add(new SqlParameter("@C_Group", SqlDbType.VarChar));
            command.Parameters["@C_Group"].Value = form.Current.BazookaInfo.Group;
            command.Parameters.Add(new SqlParameter("@C_ManagerID", SqlDbType.Int));
            command.Parameters["@C_ManagerID"].Value = form.Current.BazookaInfo.ManagerID;
            command.Parameters.Add(new SqlParameter("@C_JobTemplate", SqlDbType.VarChar));
            command.Parameters["@C_JobTemplate"].Value = form.Current.BazookaInfo.JobTemplate;
            command.Parameters.Add(new SqlParameter("@C_SecurityItemRights", SqlDbType.VarChar));
            command.Parameters["@C_SecurityItemRights"].Value = form.Current.BazookaInfo.SecurityItemRights;
            command.Parameters.Add(new SqlParameter("@C_DeskNumber", SqlDbType.VarChar));
            command.Parameters["@C_DeskNumber"].Value = form.Current.DeskInfo.DeskNumber;
            command.Parameters.Add(new SqlParameter("@C_Office", SqlDbType.VarChar));
            command.Parameters["@C_Office"].Value = form.Current.DeskInfo.Office;
            command.Parameters.Add(new SqlParameter("@C_PhoneNumber", SqlDbType.VarChar));
            command.Parameters["@C_PhoneNumber"].Value = form.Current.PhoneInfo.PhoneNumber;
            command.Parameters.Add(new SqlParameter("@C_Other", SqlDbType.VarChar));
            command.Parameters["@C_Other"].Value = form.Current.UltiproInfo.Other;
            command.Parameters.Add(new SqlParameter("@F_JobTitle", SqlDbType.VarChar));
            command.Parameters["@F_JobTitle"].Value = form.Future.BazookaInfo.JobTitle;
            command.Parameters.Add(new SqlParameter("@F_Department", SqlDbType.VarChar));
            command.Parameters["@F_Department"].Value = form.Future.BazookaInfo.Department;
            command.Parameters.Add(new SqlParameter("@F_Group", SqlDbType.VarChar));
            command.Parameters["@F_Group"].Value = form.Future.BazookaInfo.Group;
            command.Parameters.Add(new SqlParameter("@F_ManagerID", SqlDbType.Int));
            command.Parameters["@F_ManagerID"].Value = form.Future.BazookaInfo.ManagerID;
            command.Parameters.Add(new SqlParameter("@F_JobTemplate", SqlDbType.VarChar));
            command.Parameters["@F_JobTemplate"].Value = form.Future.BazookaInfo.JobTemplate;
            command.Parameters.Add(new SqlParameter("@F_SecurityItemRights", SqlDbType.VarChar));
            command.Parameters["@F_SecurityItemRights"].Value = form.Future.BazookaInfo.SecurityItemRights;
            command.Parameters.Add(new SqlParameter("@F_DeskNumber", SqlDbType.VarChar));
            command.Parameters["@F_DeskNumber"].Value = form.Future.DeskInfo.DeskNumber;
            command.Parameters.Add(new SqlParameter("@F_Office", SqlDbType.VarChar));
            command.Parameters["@F_Office"].Value = form.Future.DeskInfo.Office;
            command.Parameters.Add(new SqlParameter("@F_PhoneNumber", SqlDbType.VarChar));
            command.Parameters["@F_PhoneNumber"].Value = form.Future.PhoneInfo.PhoneNumber;
            command.Parameters.Add(new SqlParameter("@F_Other", SqlDbType.VarChar));
            command.Parameters["@F_Other"].Value = form.Future.UltiproInfo.Other;
            command.Parameters.Add(new SqlParameter("@EmailListsToBeAddedTo", SqlDbType.VarChar));
            command.Parameters["@EmailListsToBeAddedTo"].Value = form.EmailInfo.GroupsToBeAddedTo.ToString();
            command.Parameters.Add(new SqlParameter("@EmailListsToBeRemovedFrom", SqlDbType.VarChar));
            command.Parameters["@EmailListsToBeRemovedFrom"].Value = form.EmailInfo.GroupsToBeRemovedFrom.ToString();
            command.Parameters.Add(new SqlParameter("@FilesToBeAddedTo", SqlDbType.VarChar));
            command.Parameters["@FilesToBeAddedTo"].Value = form.ReviewInfo.FilesToBeAddedTo.ToString();
            command.Parameters.Add(new SqlParameter("@FilesToBeRemovedFrom", SqlDbType.VarChar));
            command.Parameters["@FilesToBeRemovedFrom"].Value = form.ReviewInfo.FilesToBeRemovedFrom.ToString();
            command.Parameters.Add(new SqlParameter("@CreateByID", SqlDbType.VarChar));
            command.Parameters["@CreateByID"].Value = 301758;
            command.Parameters.Add(new SqlParameter("@UpdateByID", SqlDbType.VarChar));
            command.Parameters["@UpdateByID"].Value = 301758;
            command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.VarChar));
            command.Parameters["@UniqueID"].Value = form.uniqueId.ToString();

            return command;
        }

        public bool UpdateRequestToServiceDeskApproved(Guid UniqueRequestID)
        {
            return UpdateRequestToApprovedStatus(UniqueRequestID, "ServiceDesk");
        }

        public bool UpdateRequestToHRApproved(Guid UniqueRequestID)
        {
            return UpdateRequestToApprovedStatus(UniqueRequestID, "HR");
        }
    }
}
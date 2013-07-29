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
        public void StoreRequestFormInDatabaseAsPending(RequestForm form)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            //get the sql command with all the parameters
            SqlCommand command = AddParametersForStoreRequestFormInDatabaseHelper(form);

            command.Connection = connection;

            //probably wrap a try/catch around this...
            command.Connection.Open();

            int result = command.ExecuteNonQuery();

            command.Connection.Close();

            if (result != 1)
            {
                //error
            }
            
        }

        /// <summary>
        /// Once the request has been approved by HR and service desk, then we need to mark that in the database
        /// Given the requestid, find the request and change the pending to false and approved to true
        /// Do we have to keep a date of when it was approved(i.e. when the last of HR or service desk approved)? when it was marked as approve in the database?
        /// </summary>
        public void UpdateRequestToApprovedStatus(int RequestID)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            SqlCommand command = new SqlCommand("UPDATE Intern_CoyoteMoves.dbo.RequestData SET Pending='0', Approved='1', WHERE RequestID="+ RequestID);

            command.Connection = connection;

            command.Connection.Open();

            int result = command.ExecuteNonQuery();

            command.Connection.Close();

            if (result != 1)
            {
                //error
            }
        }

        /// <summary>
        /// Since we have to add hella parameters to the stored proc, might as well create it's own function to do so
        /// </summary>
        /// <param name="commandString"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        private SqlCommand AddParametersForStoreRequestFormInDatabaseHelper(RequestForm form)
        {
            SqlCommand command = new SqlCommand("EXEC dbo.spRequestData_StoreRequestAsPending");

            command.Parameters.AddWithValue("@EmployeeID", form.EmployeeId);
            command.Parameters.AddWithValue("@C_JobTitle", form.Current.BazookaInfo.JobTitle);
            command.Parameters.AddWithValue("@C_Department", form.Current.BazookaInfo.Department);
            command.Parameters.AddWithValue("@C_Group", form.Current.BazookaInfo.Group);
            command.Parameters.AddWithValue("@C_Manager", form.Current.BazookaInfo.Manager);
            command.Parameters.AddWithValue("@C_JobTemplate", form.Current.BazookaInfo.JobTemplate);
            command.Parameters.AddWithValue("@C_SecurityItemRights", form.Current.BazookaInfo.SecurityItemRights);
            command.Parameters.AddWithValue("@C_DeskNumber", form.Current.DeskInfo.DeskNumber);
            command.Parameters.AddWithValue("@C_Office", form.Current.DeskInfo.Office);
            command.Parameters.AddWithValue("@C_PhoneNumber", form.Current.PhoneInfo.PhoneNumber);
            command.Parameters.AddWithValue("@C_Other", form.Current.UltiproInfo.Other);
            command.Parameters.AddWithValue("@F_JobTitle", form.Future.BazookaInfo.JobTitle);
            command.Parameters.AddWithValue("@F_Department", form.Future.BazookaInfo.Department);
            command.Parameters.AddWithValue("@F_Group", form.Future.BazookaInfo.Group);
            command.Parameters.AddWithValue("@F_Manager", form.Future.BazookaInfo.Manager);
            command.Parameters.AddWithValue("@F_JobTemplate", form.Future.BazookaInfo.JobTemplate);
            command.Parameters.AddWithValue("@F_SecurityItemRights", form.Future.BazookaInfo.SecurityItemRights);
            command.Parameters.AddWithValue("@F_DeskNumber", form.Future.DeskInfo.DeskNumber);
            command.Parameters.AddWithValue("@F_Office", form.Future.DeskInfo.Office);
            command.Parameters.AddWithValue("@F_PhoneNumber", form.Future.PhoneInfo.PhoneNumber);
            command.Parameters.AddWithValue("@F_Other", form.Future.UltiproInfo.Other);
            //for the rest I'm passing in lists of strings... which probably isn't good?
            command.Parameters.AddWithValue("@EmailListsToBeAddedTo", form.EmailGroupsToBeAddedTo);
            command.Parameters.AddWithValue("@EmailListsToBeRemovedFrom", form.EmailGroupsToBeAddedTo);
            command.Parameters.AddWithValue("@FilesToBeAddedTo", form.FilesToBeAddedTo);
            command.Parameters.AddWithValue("@FilesToBeRemovedFrom", form.FilesToBeRemovedFrom);
            

            return command;
        }
    }
}
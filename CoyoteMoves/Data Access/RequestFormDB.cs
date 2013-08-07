using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoyoteMoves.Models.RequestItems;
using System.Data.SqlClient;
using System.Data;
using CoyoteMoves.Models;

namespace CoyoteMoves.Data_Access
{
    public class RequestFormDB
    {
        public string _connectionString { get; set; }
        public SqlToFormModelFactory _factory;

        public RequestFormDB()
        {
            _connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }

        public bool StoreRequestFormInDatabaseAsPending(RequestForm form)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = AddParametersForStoreRequestFormInDatabaseHelper(form);
            command.Connection = connection;
            command.Connection.Open();
            int result = command.ExecuteNonQuery();
            command.Connection.Close();

            return (result == 1);
            
        }

        private bool UpdateRequestToApprovedStatus(Guid UniqueRequestID, string ApprovalDept)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand("UPDATE Intern_CoyoteMoves.dbo.RequestData SET " + ApprovalDept +"=1 WHERE UniqueRequestID=@UniqueRequestID");
            command.Parameters.Add("@UniqueRequestID", SqlDbType.UniqueIdentifier).Value = UniqueRequestID;
            command.Connection = connection;
            command.Connection.Open();

            int result = command.ExecuteNonQuery();//why is it trying to access the guid string? 
            command.Connection.Close();

            bool theOtherDepartmentHasApproved = CheckOtherDepartmentApproval(UniqueRequestID, ApprovalDept);
            if (theOtherDepartmentHasApproved)
            {
                setTheRequestAsNotPending(UniqueRequestID);
            }

            return (result == 1);

        }
        private void setTheRequestAsNotPending(Guid uniqueRequestID)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Intern_CoyoteMoves.dbo.RequestData SET Pending = 0 WHERE UniqueRequestID = @guid");
            cmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier).Value = uniqueRequestID;
            cmd.Connection = connection;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private bool CheckOtherDepartmentApproval(Guid uniqueRequestID, string ApprovalDept)
        {
            bool approved = false;
            string OtherDept = ApprovalDept.Equals("HRApproved") ? "ServiceDeskApproved" : "HRApproved";
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("SELECT " + OtherDept + " FROM dbo.RequestData WHERE UniqueRequestID= @guid");
            cmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier).Value = uniqueRequestID;

            cmd.Connection = connection;
            cmd.Connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                approved = (bool)(reader[OtherDept]); 
            }

            connection.Close();
            return approved;
        }

        private SqlCommand AddParametersForStoreRequestFormInDatabaseHelper(RequestForm form)
        {

            string commandString = "[Intern_CoyoteMoves].[dbo].[spRequestData_StoreRequestAsPending]";
            SqlCommand command = new SqlCommand(commandString);
            command.CommandType = System.Data.CommandType.StoredProcedure;

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
            command.Parameters.AddWithValue("@EmailListsToBeAddedTo", form.EmailInfo.GroupsToBeAddedTo);
            command.Parameters.Add(new SqlParameter("@EmailListsToBeRemovedFrom", SqlDbType.VarChar));
            command.Parameters["@EmailListsToBeRemovedFrom"].Value = form.EmailInfo.GroupsToBeRemovedFrom;
            command.Parameters.Add(new SqlParameter("@FilesToBeAddedTo", SqlDbType.VarChar));
            command.Parameters["@FilesToBeAddedTo"].Value = form.ReviewInfo.FilesToBeAddedTo;
            command.Parameters.Add(new SqlParameter("@FilesToBeRemovedFrom", SqlDbType.VarChar));
            command.Parameters["@FilesToBeRemovedFrom"].Value = form.ReviewInfo.FilesToBeRemovedFrom;
            command.Parameters.Add(new SqlParameter("@CreateByID", SqlDbType.VarChar));
            command.Parameters["@CreateByID"].Value = 301758;
            command.Parameters.Add(new SqlParameter("@UpdateByID", SqlDbType.VarChar));
            command.Parameters["@UpdateByID"].Value = 301758;
            command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.VarChar));
            command.Parameters["@UniqueID"].Value = form.UniqueId.ToString();

            return command;
        }

        public bool UpdateRequestToServiceDeskApproved(Guid UniqueRequestID)
        {
            return UpdateRequestToApprovedStatus(UniqueRequestID, "ServiceDeskApproved");
        }

        public bool UpdateRequestToHRApproved(Guid UniqueRequestID)
        {
            return UpdateRequestToApprovedStatus(UniqueRequestID, "HRApproved");
        }

        public bool RemoveRequestByUniqueId(Guid UniqueRequestID)
        {
            SqlConnection connection = new SqlConnection(this._connectionString);
            string commandString = "DELETE FROM [Intern_CoyoteMoves].[dbo].[RequestData] WHERE UniqueRequestID = @UniqueRequestId";
            SqlCommand command = new SqlCommand(commandString);
            command.Parameters.AddWithValue("@UniqueRequestId", UniqueRequestID);
            command.Connection = connection;
            command.Connection.Open();
            int result = command.ExecuteNonQuery();
            command.Connection.Close();

            return (result == 1);
        }

        public bool HRApproved(Guid uniqueRequestID)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("SELECT HRApproved FROM RequestData WHERE UniqueRequestID = @guid");
            cmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier).Value = uniqueRequestID;
            cmd.Connection = connection;
            connection.Open();
            var approval = cmd.ExecuteScalar();

            connection.Close();
            if ((approval == null) || (approval == DBNull.Value))
            {
                return false;
            }
            return (bool)approval;
        }

        public bool SDApproved(Guid uniqueRequestID)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("SELECT ServiceDeskApproved FROM RequestData WHERE UniqueRequestID = @guid");
            cmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier).Value = uniqueRequestID;
            cmd.Connection = connection;
            connection.Open();
            var approval = cmd.ExecuteScalar();

            connection.Close();
            if ((approval == null) || (approval == DBNull.Value))
            {
                return false;
            }
            return (bool)approval;
        }

        public RequestForm RetrieveRequest (Guid uniqueRequestID)
        {
            RequestForm toReturn = new RequestForm();
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "EXEC dbo.spRequestData_GetRequestDataByUniqueID @guid = @druid";
            SqlCommand command = new SqlCommand(commandString);

            command.Parameters.AddWithValue("@druid", uniqueRequestID);
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            SqlToFormModelFactory RequestFactory = new SqlToFormModelFactory(reader);
            toReturn = RequestFactory.GetRequest(uniqueRequestID);
            connection.Close();
            return toReturn;
        }
    }
}
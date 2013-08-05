using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Models;
using CoyoteMoves.Models.RequestItems;

namespace CoyoteMoves.Data_Access
{
    public class InfoValidator
    {

        public string _connectionString { get; set; }

        public InfoValidator()
        {
            this._connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }

        /// <summary>
        /// Given the name of the future manager, check to make sure that that manager is at least a real employee
        /// Will probably run into problems when two people have the same name...
        /// </summary>
        public bool ValidateManager(string name)
        {
           

            string firstName = name.Split(' ').First();
            string lastName = name.Split(' ').Last();

            return true;
        }

        /// <summary>
        /// Just checks that the desk number is an actual desk number in the database
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool ValidateDeskNumber(string number)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand("Select DeskNumber from Intern_CoyoteMoves.dbo.Desk where DeskNumber='" + number + "'");
            command.Connection = connection;
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader["DeskNumber"].ToString() == number)
                {
                    command.Connection.Close();
                    return true;
                }
            }

            command.Connection.Close();
            return false;
        }

        public bool ValidateJobTemplate(string template)
        {
            return true;
        }

        public bool ValidateServiceDeskApproval(Guid requestId)
        {
            bool toReturn = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand("Select ServiceDeskApproved from Intern_CoyoteMoves.dbo.RequestData where UniqueRequestID = @requestID");
            command.Parameters.AddWithValue("@requestID", requestId);
            command.Connection = connection;
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                toReturn = (bool)reader["ServiceDeskApproved"];
            }

            command.Connection.Close();
            return toReturn;
        }

        public bool ValidateHumanResourcesApproval(Guid requestId)
        {
            bool toReturn = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand("Select HRApproved from Intern_CoyoteMoves.dbo.RequestData where UniqueRequestID = @requestID");
            command.Parameters.AddWithValue("@requestID", requestId);
            command.Connection = connection;
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                toReturn = (bool)reader["HRApproved"];
            }

            command.Connection.Close();
            return toReturn;

        }

        public bool ValidateRequestForm(RequestForm req)
        {
            bool toReturn = true;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand("Select * from Intern_CoyoteMoves.dbo.RequestData where UniqueRequestID = @uniqueRequestID");
            command.Parameters.AddWithValue("@uniqueRequestID", req.UniqueId);
            command.Connection = connection;
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                toReturn = (reader["EmployeeID"].ToString() == req.EmployeeId.ToString());
                toReturn = (reader["C_DeskNumber"].ToString() == req.Current.DeskInfo.DeskNumber);
                toReturn = (reader["F_DeskNumber"].ToString() == req.Future.DeskInfo.DeskNumber);
                toReturn = (reader["C_JobTitleID"].ToString() == req.Current.BazookaInfo.JobTitle);
                toReturn = (reader["C_DepartmentID"].ToString() == req.Current.BazookaInfo.Department);
                toReturn = (reader["C_GroupID"].ToString() == req.Current.BazookaInfo.Group);
                toReturn = (reader["C_JobTemplate"].ToString() == req.Current.BazookaInfo.JobTemplate);
                toReturn = (reader["C_SecurityItemRights"].ToString() == req.Current.BazookaInfo.SecurityItemRights);
                toReturn = (reader["C_ManagerID"].ToString() == req.Current.BazookaInfo.ManagerID.ToString());
                toReturn = (reader["F_JobTitleID"].ToString() == req.Future.BazookaInfo.JobTitle);
                toReturn = (reader["F_DepartmentID"].ToString() == req.Future.BazookaInfo.Department);
                toReturn = (reader["F_GroupID"].ToString() == req.Future.BazookaInfo.Group);
                toReturn = (reader["F_JobTemplate"].ToString() == req.Future.BazookaInfo.JobTemplate);
                toReturn = (reader["F_SecurityItemRights"].ToString() == req.Future.BazookaInfo.SecurityItemRights);
                toReturn = (reader["F_ManagerID"].ToString() == req.Future.BazookaInfo.ManagerID.ToString());
                toReturn = (reader["C_Office"].ToString() == req.Current.DeskInfo.Office);
                toReturn = (reader["F_Office"].ToString() == req.Future.DeskInfo.Office);
                toReturn = (reader["GroupsToBeAddedTo"].ToString() == req.EmailInfo.GroupsToBeAddedTo);
                toReturn = (reader["GroupsToBeRemovedFrom"].ToString() == req.EmailInfo.GroupsToBeRemovedFrom);
                toReturn = (reader["FilesToBeAddedTo"].ToString() == req.ReviewInfo.FilesToBeAddedTo);
                toReturn = (reader["FilesToBeRemovedFrom"].ToString() == req.ReviewInfo.FilesToBeRemovedFrom);
                toReturn = (reader["C_PhoneNumber"].ToString() == req.Current.PhoneInfo.PhoneNumber);
                toReturn = (reader["F_PhoneNumber"].ToString() == req.Future.PhoneInfo.PhoneNumber);
            }

            command.Connection.Close();
            return toReturn;

        }
    }
}
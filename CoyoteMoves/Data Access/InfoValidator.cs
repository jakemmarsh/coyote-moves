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
            SqlCommand command = new SqlCommand("Select ServiceDeskApproved from Intern_CoyoteMoves.dbo.RequestData where RequestID = @requestID");
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
            SqlCommand command = new SqlCommand("Select HRApproved from Intern_CoyoteMoves.dbo.RequestData where RequestID = @requestID");
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
            SqlCommand command = new SqlCommand("Select UniqueRequestID from Intern_CoyoteMoves.dbo.RequestData where UniqueRequestID = @uniqueRequestId");
            command.Parameters.AddWithValue("@uniqueRequestID", req.UniqueId);
            command.Connection = connection;
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                toReturn = (reader["EmployeeID"] == req.EmployeeId.ToString());
                toReturn = 
                toReturn = (bool)reader["HRApproved"];
            }

            command.Connection.Close();
            return toReturn;

        }
    }
}
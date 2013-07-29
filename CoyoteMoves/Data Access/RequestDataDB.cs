using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using CoyoteMoves.Models;
using CoyoteMoves.Models.EmployeeData;
using System.Collections.ObjectModel;

namespace CoyoteMoves.Data_Access
{
    public class RequestDataDB
    {
        string _connectionString;

        public RequestDataDB()
        {
            _connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }

        public List<string> GetAllGroups()
        {
            List<string> groupTypes = new List<string>();
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "SELECT [Code] FROM [dbo].[GroupType]";
            SqlCommand command = new SqlCommand(commandString);
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                groupTypes.Add(reader["Code"].ToString());
            }
            connection.Close();
            return groupTypes;
        }

        public List<string> GetAllDepartments()
        {
            List<string> departments = new List<string>();
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "SELECT [Code] FROM [dbo].[Department]";
            SqlCommand command = new SqlCommand(commandString);
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                departments.Add(reader["Code"].ToString());
            }
            connection.Close();
            return departments;
        }

        public List<string> GetAllJobTitles()
        {
            List<string> jobTitles = new List<string>();
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "SELECT [Code] FROM [dbo].[JobTitle]";
            SqlCommand command = new SqlCommand(commandString);
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                jobTitles.Add(reader["Code"].ToString());
            }
            connection.Close();
            return jobTitles;
        }
    }
}
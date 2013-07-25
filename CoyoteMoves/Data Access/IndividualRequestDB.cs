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
    public class IndividualRequestDB
    {
        string _connectionString;

        public IndividualRequestDB()
        {
            _connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;

        }

        public string GetFullNameById(int Id)
        {
            Collection<string> nameCollection = new Collection<string>();
            
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "SELECT [FirstName], [LastName] FROM [Intern_CoyoteMoves].[dbo].[Person] WHERE ([PersonID] = @Id)";
            SqlCommand command = new SqlCommand(commandString);
            command.Parameters.AddWithValue("@Id", Id);
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                nameCollection.Add(reader["FirstName"] + " " + reader["LastName"]); 
            }

            return nameCollection[0];
        }

        public Employee GetEmployeeById(int Id)
        {
            //Collection<string> 
            //Need to account for template and security settings - refactor employee object or find them
            return null;
        }
                
        public Collection<int> GetEmployeeIdsByGroupId(int groupId)
        {
            Collection<int> returnToSender = new Collection<int>();

            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "SELECT [EmployeeID] FROM [Intern_CoyoteMoves].[dbo].[InternalEmployee] WHERE ([Group] = @Id)";
            SqlCommand command = new SqlCommand(commandString);
            command.Parameters.AddWithValue("@Id", groupId);        //#fuckmicrosoft
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                returnToSender.Add((int)reader["EmployeeID"]);
            }
            connection.Close();

            return returnToSender;

        }

    }
}
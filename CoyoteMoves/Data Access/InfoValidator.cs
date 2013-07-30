using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Models;

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
        /// Returns a list of Employees with the same first and last name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Employee> GetAllEmployeesWithSameFullName(string name)
        {
            List<Employee> employeesWithName = new List<Employee>();
            SqlConnection connection = new SqlConnection(_connectionString);

            //command should be a stored proc
            string commandString = "EXEC dbo.spEmployee_GetEmployeesWithName";
            SqlCommand command = new SqlCommand(commandString);
            command.Parameters.AddWithValue("@FirstName", name.Split(' ').First());
            command.Parameters.AddWithValue("@LastName", name.Split(' ').Last());
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            SqlToModelFactory factory = new SqlToModelFactory(reader);

            //execute the stored proc
            //turn the results into a list of employees
            while (reader.Read())
            {
                employeesWithName.Add(factory.CreateEmployee());
            }

            connection.Close();
            return employeesWithName;
        }


        /// <summary>
        /// Just checks that the desk number is an actual desk number in the database
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool ValidateDeskNumber(string number)
        {
            //set up a connection and look in dbo.Desk for something where DeskNumber = number
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
            //wtf is jobtemplate
            return true;
        }
    }
}
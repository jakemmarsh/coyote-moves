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
        /// Might have to make sure they're also a manager too? Idk how we'd do that
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ValidateManager(string name)
        {
            //wtf is manager id? is that the id of that person's manager? or is that the id of that person AS manager?

            /*
             * So, the user is entering in plain text of a name of a manager. Multiple people can have the same name,
             * we should do something to make sure that the user can select which person they want. e.g. if there are two john doe's, then
             * when they type in john doe, something will appear presenting a little bit more information about each john doe and force the user to choose
             * one of them. Then after they choose, we'll have more than just a name and have the person ID so we'll know exactly who they're talking about
             * */

            /*
             * So if we did that, then once the user types in a name, the front end will send us the name, we'll go into the DB and get all the
             * employees with that name and a little bit of other information for the user (e.g. department, job title w/e) and for us (employee id) on
             * each match. Then send that to the front end to present those choices to the user and deal with that. This means, when the front end
             * sends the form back to us via the "submit", we'll know the manager will be legit. the front end will have to send us the id of the manager though
             * */

            string firstName = name.Split(' ').First();
            string lastName = name.Split(' ').Last();

            return true;
        }

        /// <summary>
        /// 
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
            int result = command.ExecuteNonQuery();
            command.Connection.Close();

            //should we check that it's not zero? or that it only returns one row affected?
            if (result != 1)
            {
                return false;
            }
            return true;
        }

        public bool ValidateJobTemplate(string template)
        {
            //wtf is jobtemplate
            return true;
        }
    }
}
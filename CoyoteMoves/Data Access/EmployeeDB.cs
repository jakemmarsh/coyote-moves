using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Data_Access
{
    public class EmployeeDB
    {
        private string _connectionString;

        public EmployeeDB()
        {
            _connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }
       
        public object GetIdFromName(string name)
        {
            string[] names = name.Split(' ');
            int Id =-1;
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "select PersonID from dbo.Person AS P where P.FirstName = @Fname AND P.LastName = @Lname";
            SqlCommand command = new SqlCommand(commandString);

            try
            {
                command.Parameters.AddWithValue("@Fname", names[0]);
                command.Parameters.AddWithValue("@Lname", names[1]);
                command.Connection = connection;
                connection.Open();
                Id = (int)command.ExecuteScalar();
                connection.Close();
                return Id;
            }
            
            catch(NullReferenceException ex)
            {
                //nothing was returned! ;_;
                return -1;
            }
        }
    }
}
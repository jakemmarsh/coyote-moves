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
       
        public int GetIdFromName(string name)
        {
            string[] names = name.Split(' ');
            string lastname = ""; //this is needed for cases where
                                  //the employee has a last name with
                                  //spaces i.e. 'van dyke'
            for (int i = 1; i < names.Length; i++)
            {
                if (i == 1)
                {
                    lastname += names[i];
                }
                else
                {
                    lastname += " " + names[i];
                }
            }
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "select PersonID from dbo.Person AS P where P.FirstName = @Fname AND P.LastName = @Lname";
            SqlCommand command = new SqlCommand(commandString);

            try
            {
                command.Parameters.AddWithValue("@Fname", names[0]);
                command.Parameters.AddWithValue("@Lname", lastname);
                command.Connection = connection;
                connection.Open();
                var temp = command.ExecuteScalar();
                connection.Close();
                if ((temp == null) || (temp == DBNull.Value))
                {
                    return -1;
                }
                return (int)temp;
            }

            catch (NullReferenceException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
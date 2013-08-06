using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Data_Access
{
    public class AnalyticsDB
    {private string _connectionString;
        public AnalyticsDB()
        {  

        public EmployeeDB()
        {
            _connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }
       

        }

        public bool GetAllRequestRecords()
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
    }
}
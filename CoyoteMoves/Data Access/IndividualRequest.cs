using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace CoyoteMoves.Data_Access
{
    public class IndividualRequest
    {
        string connectionString = "server=AnalyticsProd;database=Intern_CoyoteMoves;User Id=Intern;Password=Intern2013!;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";

        public IndividualRequest()
        {
           // connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;

        }

        public int test()
        {
            SqlConnection myConnection = new SqlConnection(connectionString);
            string testQuery = "SELECT TOP 1 [EmployeeID] FROM [Intern_CoyoteMoves].[dbo].[InternalEmployee]";
            SqlCommand myCommand = new SqlCommand(testQuery);
            myCommand.Connection = myConnection;
            myConnection.Open();
            int pleasebe46 = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return pleasebe46;

        }

    }
}
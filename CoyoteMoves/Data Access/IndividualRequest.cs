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
        string _connectionString;

        public IndividualRequest()
        {
            _connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;

        }

        public int GenericIntCall(string query) //all you get is 1 int
        {
            SqlConnection myConnection = new SqlConnection(_connectionString);
            //string testQuery = "SELECT TOP 1 [EmployeeID] FROM [Intern_CoyoteMoves].[dbo].[InternalEmployee]";
            SqlCommand myCommand = new SqlCommand(query);
            myCommand.Connection = myConnection;
            myConnection.Open();
            int intCall = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return intCall;

        }

    }
}
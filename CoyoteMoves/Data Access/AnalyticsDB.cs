using CoyoteMoves.Models;
using CoyoteMoves.Models.RequestItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Data_Access
{
    public class AnalyticsDB
    {
        private string _connectionString;
        
        public AnalyticsDB()
        {  
            _connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }

        public Collection<RequestForm> GetAllRequestRecords()
        {
            return GetRequestsBetweenDates(DateTime.MinValue, DateTime.MaxValue);
        }

        public Collection<RequestForm> GetRequestsBetweenDates(DateTime begin, DateTime end)
        {
            Collection<RequestForm> requestCollection = new Collection<RequestForm>();
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "SELECT [UniqueRequestID], [CreateDate] FROM [Intern_CoyoteMoves].[dbo].[RequestData] WHERE [CreateDate] < @end AND [CreateDate] > @begin";
            SqlCommand command = new SqlCommand(commandString);

            command.Parameters.AddWithValue("@begin", begin);
            command.Parameters.AddWithValue("@end", end);
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            SqlToModelFactory RequestFactory = new SqlToModelFactory(reader);

            //requestList = RequestFactory.RetrieveRequest(;
            connection.Close();
            return requestCollection;
        }

        
    }
}
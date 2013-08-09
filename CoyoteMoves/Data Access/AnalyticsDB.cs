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
        private RequestFormDB _requester;
        
        public AnalyticsDB()
        {
            _requester = new RequestFormDB();
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
            string commandString = "SELECT [UniqueRequestID], [CreateDate] FROM [Intern_CoyoteMoves].[dbo].[RequestData] WHERE [CreateDate] <= @end AND [CreateDate] >= @begin";
            SqlCommand command = new SqlCommand(commandString);

            command.Parameters.AddWithValue("@begin", begin);
            command.Parameters.AddWithValue("@end", end);
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            SqlToFormModelFactory RequestFactory = new SqlToFormModelFactory(reader);
            while (reader.Read())
            {
                requestCollection.Add(_requester.RetrieveRequest((Guid)reader["UniqueRequestID"]));
            }
            connection.Close();
            return requestCollection;
        }

        
    }
}
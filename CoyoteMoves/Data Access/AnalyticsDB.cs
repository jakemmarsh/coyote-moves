using CoyoteMoves.Models;
using CoyoteMoves.Models.RequestItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Data_Access
{
    public class AnalyticsDB
    {
        private string _connectionString;
        private RequestFormDB _requester;
        private RequestDataDB _dataRequest;
        
        public AnalyticsDB()
        {
            _requester = new RequestFormDB();
            _dataRequest = new RequestDataDB();
            _connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }

        public Collection<RequestForm> GetAllApprovedRequests()
        {
            return GetApprovedRequestsBetweenDates(SqlDateTime.MinValue, SqlDateTime.MaxValue);
        }

        public Collection<RequestForm> GetApprovedRequestsBetweenDates(SqlDateTime begin, SqlDateTime end)
        {
            Collection<RequestForm> requestCollection = new Collection<RequestForm>();
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "SELECT [UniqueRequestID] FROM [Intern_CoyoteMoves].[dbo].[RequestData] WHERE [CreateDate] <= @end AND [CreateDate] >= @begin AND [Pending] = 0";
            SqlCommand command = new SqlCommand(commandString);

            command.Parameters.AddWithValue("@begin", begin);
            command.Parameters.AddWithValue("@end", end);
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            SqlToFormModelFactory RequestFactory = new SqlToFormModelFactory(reader);
            while (reader.Read())
            {
                requestCollection.Add(_requester.RetrieveRequest(reader.GetGuid(reader.GetOrdinal("UniqueRequestID"))));
            }
            connection.Close();
            return requestCollection;
        }

        public string GetGroupChangeInformationBetweenDates(SqlDateTime begin, SqlDateTime end)
        {
            Collection<RequestForm> forms = GetApprovedRequestsBetweenDates(begin, end);
            List<string> groups = _dataRequest.GetAllGroups();
            Dictionary<string, int> groupCount = new Dictionary<string, int>();

            foreach (string entry in groups)
            {
                groupCount.Add(entry, 0);
            }

            foreach (RequestForm entry in forms)
            {
                string pastGroup = entry.Current.BazookaInfo.Group;
                string futureGroup = entry.Future.BazookaInfo.Group;
                if (groupCount.ContainsKey(pastGroup))
                    groupCount[pastGroup]--;

                if (groupCount.ContainsKey(futureGroup))
                    groupCount[futureGroup]++;
            }

            return groupCount.ToString();
        }

        public string GetAllGroupChangeInformation()
        {
            return GetGroupChangeInformationBetweenDates(SqlDateTime.MinValue, SqlDateTime.MaxValue);
        }

        
    }
}
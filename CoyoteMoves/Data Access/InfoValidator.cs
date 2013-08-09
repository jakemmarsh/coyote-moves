using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Models;
using CoyoteMoves.Models.RequestItems;

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
        /// checks that the desk number is an actual desk number in the database
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool ValidateDeskNumber(string number)
        {
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

        public bool ValidateDeskInfo(string number, double topLeftX, double topLeftY, double orient)
        {
            bool deskReturn = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand("Select [DeskNumber], [TopLeftX], [TopLeftY], [Orientation] from Intern_CoyoteMoves.dbo.Desk where DeskNumber=@deskNum");
            command.Parameters.AddWithValue("@deskNum", number);
            command.Connection = connection;
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                deskReturn = (reader["DeskNumber"].ToString() == number);
                deskReturn = (reader["TopLeftX"].ToString() == topLeftX.ToString());
                deskReturn = (reader["TopLeftY"].ToString() == topLeftY.ToString());
                deskReturn = (reader["Orientation"].ToString() == orient.ToString());
            }

            command.Connection.Close();
            return deskReturn;

        }

        public bool ValidateFirstAndLastRequests(DateTime begin, DateTime end)
        {
            bool requestReturn = false;
            //SqlConnection connection = new SqlConnection(_connectionString);
            //SqlCommand command = new SqlCommand("Select [DeskNumber], [TopLeftX], [TopLeftY], [Orientation] from Intern_CoyoteMoves.dbo.Desk where DeskNumber=@deskNum");
            //command.Parameters.AddWithValue("@deskNum", number);
            //command.Connection = connection;
            //command.Connection.Open();
            //SqlDataReader reader = command.ExecuteReader();
            //while (reader.Read())
            //{
            //    deskReturn = (reader["DeskNumber"].ToString() == number);
            //    deskReturn = (reader["TopLeftX"].ToString() == topLeftX.ToString());
            //    deskReturn = (reader["TopLeftY"].ToString() == topLeftY.ToString());
            //    deskReturn = (reader["Orientation"].ToString() == orient.ToString());
            //}

            //command.Connection.Close();
            //return deskReturn;
            return requestReturn;


        }
    }
}
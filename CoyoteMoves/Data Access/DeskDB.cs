using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using CoyoteMoves.Models;
using CoyoteMoves.Models.EmployeeData;
using System.Collections.ObjectModel;
using CoyoteMoves.Models.SeatingData;

namespace CoyoteMoves.Data_Access
{
    public class DeskDB
    {
        private string _connectionString;

        public DeskDB()
        {
            _connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }
        /// <summary>
        /// Function to query the database for 
        /// desks on a particular floor
        /// </summary>
        public List<Desk> GetDesksFromFloor(int floor)
        {
            List<Desk> deskList = new List<Desk>();
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "EXEC dbo.spDesk_GetDesksByFloor @FloorNumber = @floor";
            SqlCommand command = new SqlCommand(commandString);

            try
            {
                command.Parameters.AddWithValue("@floor", floor);
                command.Connection = connection;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                SqlToModelFactory DeskFactory = new SqlToModelFactory(reader);
                deskList = DeskFactory.GetAllDesks(floor);
                connection.Close();
                return deskList;
            }
            
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
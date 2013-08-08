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
using System.IO;

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

                SqlToDeskModelFactory DeskFactory = new SqlToDeskModelFactory(reader);
                deskList = DeskFactory.GetAllDesks(floor);
                connection.Close();
                return deskList;
            }
            
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool InsertInformationIntoDeskDB(string deskNumber, int topLeftX, int topLeftY, int orientation, int floorNumber, int employeeID)
        {
            SqlConnection connection = new SqlConnection(this._connectionString);
            string commandString = "INSERT INTO Intern_CoyoteMoves.dbo.Desk (DeskNumber, TopLeftX, TopLeftY, Orientation, FloorNumber, EmployeeID) Values (@DeskNumber, @TopLeftX, @TopLeftY, @Orientation, @FloorNumber, @EmployeeID);";
           
            SqlCommand command = new SqlCommand(commandString); 
            command.Parameters.AddWithValue("@DeskNumber", deskNumber);
            command.Parameters.AddWithValue("@TopLeftX", topLeftX);
            command.Parameters.AddWithValue("@TopLeftY", topLeftY);
            command.Parameters.AddWithValue("@Orientation", orientation);
            command.Parameters.AddWithValue("@FloorNumber", floorNumber);
            command.Parameters.AddWithValue("@EmployeeID", employeeID);
            command.Connection = connection;
            command.Connection.Open();
            int result = command.ExecuteNonQuery();
            command.Connection.Close();

            return (result == 1);
        }

        public bool RemoveDeskFromDeskDB(string deskNumber)
        {
            SqlConnection connection = new SqlConnection(this._connectionString);
            string commandString = "DELETE FROM [Intern_CoyoteMoves].[dbo].[Desk] WHERE DeskNumber= @DeskNumber";
            SqlCommand command = new SqlCommand(commandString);
            command.Parameters.AddWithValue("@DeskNumber", deskNumber);
            command.Connection = connection;
            command.Connection.Open();
            int result = command.ExecuteNonQuery();
            command.Connection.Close();

            return (result == 1);
        }

        public bool CheckIfDeskExisits(string deskNumber)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            string command = "SELECT DeskNumber FROM [Intern_CoyoteMoves].[dbo].[Desk] WHERE DeskNumber = @num";
            SqlCommand cmd = new SqlCommand(command);

            try
            {
                cmd.Parameters.AddWithValue("@num", deskNumber);
                cmd.Connection = conn;
                conn.Open();
                object temp = cmd.ExecuteScalar();
                conn.Close();

                if((temp == null) || (temp == DBNull.Value))
                {
                    return false;
                }
                return true;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ChangeDeskPointAndOrientation(string deskNumber, double topLeftX, double topLeftY, double orientation)
        {
            if (!CheckIfDeskExisits(deskNumber))
            {
                return false;
            }
            SqlConnection connection = new SqlConnection(_connectionString);
            string commandString = "EXEC spDesk_SetDeskPointAndOrientation @x, @y, @angle, @deskNum";
            SqlCommand command = new SqlCommand(commandString);

            try
            {
                command.Parameters.AddWithValue("@x", topLeftX);
                command.Parameters.AddWithValue("@y", topLeftY);
                command.Parameters.AddWithValue("@angle", orientation);
                command.Parameters.AddWithValue("deskNum", deskNumber);
                command.Connection = connection;
                connection.Open();

                command.ExecuteNonQuery();
                return true;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
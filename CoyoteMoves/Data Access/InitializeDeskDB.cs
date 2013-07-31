using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace CoyoteMoves.Data_Access
{
    public class InitializeDeskDB
    {

        public string _connectionString { get; set; }

        public InitializeDeskDB()
        {
            this._connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }

        public void AddNamesAndDeskNumbersFromFile(string filePath)
        {
            
        }

        private void InsertInformationIntoDeskDB(string deskNumber, int topLeftX, int topLeftY, int orientation, int floorNumber, int employeeID)
        {
            SqlConnection connection = new SqlConnection(this._connectionString);
            string commandString = "INSERT INTO dbo.Desk (DeskNumber, TopLeftX, TopLeftY, Orientation, FloorNumber, EmployeeID)";
            commandString += " Values (" + "'" + deskNumber + "'" + ", " + topLeftY + ", " + orientation + ", " + floorNumber + ", " + employeeID + ")";
            SqlCommand command = new SqlCommand(commandString);
            command.Connection = connection;
            command.Connection.Open();
            int result = command.ExecuteNonQuery();
            command.Connection.Close();
            if (result != 1)
            {
                //error
            }
        }
    }
}
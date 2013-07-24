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
            command.Parameters.AddWithValue("@floor", floor);
            command.Connection = connection;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string deskNumber = reader["DeskNumber"].ToString();
                CoordinatePoint TopRight = new CoordinatePoint((double)reader["TopRightX"], (double)reader["TopRightY"]);
                CoordinatePoint TopLeft = new CoordinatePoint((double)reader["TopLeftX"], (double)reader["TopLeftY"]);
                CoordinatePoint BottomRight = new CoordinatePoint((double)reader["BottomRightX"], (double)reader["BottomRightY"]);
                CoordinatePoint BottomLeft = new CoordinatePoint((double)reader["BottomLeftX"], (double)reader["BottomLeftY"]);
                Location loc = new Location(floor, TopLeft, TopRight, BottomRight, BottomLeft);

                string FirstName = reader["FirstName"].ToString();
                string LastName = reader["LastName"].ToString();
                string Email = reader["WorkEmail"].ToString();
                string JobTitle = reader["JobTitle"].ToString();
                //template
                string Department = reader["Department"].ToString();
                string Group = reader["Group"].ToString();
                int ID = (int)reader["PersonID"];
                string ManagerName = reader["ManagerFirstName"].ToString() + " " + reader["ManagerLastName"].ToString();
                //security items rights
                Employee TempGuy = new Employee()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    JobTitle = JobTitle,
                    Department = Department,
                    Group = Group,
                    Id = ID,
                    ManagerName = ManagerName,
                };

                Desk TempDesk = new Desk(loc, deskNumber, TempGuy);
                deskList.Add(TempDesk);
            }
            connection.Close();
            return deskList;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;

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
            //this program takes like three mintues to run
            //the file that should be given to this function should be a csv text file
            //assumes that each line is like: 'firstname.lastname,desknumber,'
            //this was done so we could just download the google doc as a csv file and put that info into our database

            StreamReader reader = new StreamReader(Path.GetFullPath(filePath));
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("Name") == false)
                {
                    string[] arr = line.Split(',');
                    string employeeName = arr[0];
                    string newname = employeeName.Split('.')[0] + " " + employeeName.Split('.')[1];

                    EmployeeDB emp = new EmployeeDB();
                    int id = emp.GetIdFromName(newname);

                    //something is weird for this person INSIDE the database (ask Mitch about it), so the function above won't work, so just set it to their id
                    if (newname == "Jordan Brychell")
                    {
                        id = 10184;
                    }

                    string deskNumber = arr[1];

                    int floorNumber = Convert.ToInt32(arr[1][0].ToString());
                   
                    Console.WriteLine(newname + "\n" + "\tid: " + id + "\n\tdesk: " + deskNumber + "\n\tfloor: " + floorNumber);
                    
                    //finally, actually insert that info into the desk database
                    InsertInformationIntoDeskDB(deskNumber, 0, 0, 0, floorNumber, id);
                }
            }

            reader.Close();
        }

        private void InsertInformationIntoDeskDB(string deskNumber, int topLeftX, int topLeftY, int orientation, int floorNumber, int employeeID)
        {
            SqlConnection connection = new SqlConnection(this._connectionString);
            string commandString = "INSERT INTO Intern_CoyoteMoves.dbo.Desk (DeskNumber, TopLeftX, TopLeftY, Orientation, FloorNumber, EmployeeID)";
            commandString += " Values (" + "'" + deskNumber + "'" + ", " + topLeftX + ", " + topLeftY + ", " + orientation + ", " + floorNumber + ", " + employeeID + ")";
            SqlCommand command = new SqlCommand(commandString);
            command.Connection = connection;
            command.Connection.Open();
            int result = command.ExecuteNonQuery();
            command.Connection.Close();
            if (result != 1)
            {
                Console.WriteLine("error");
            }
        }
    }
}
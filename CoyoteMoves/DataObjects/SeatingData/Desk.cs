using CoyoteMoves.DataObjects.EmployeeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.DataObjects.SeatingData
{
    public class Desk
    {
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public Floor Floor { get; set; }
        public bool Occupied { get; set; }
        public Employee CurrentTenant { get; set; } 

        public Desk()
        {
            this.Occupied = false;
            this.XCoordinate = 0.0;
            this.YCoordinate = 0.0;
        }
    }
}
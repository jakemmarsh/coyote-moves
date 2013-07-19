using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.SeatingData
{
    public class Desk
    {
        public Location LocationData { get; set; }
        public Floor Floor { get; set; }
        public bool Occupied { get; set; }
        public Employee CurrentTenant { get; set; }

        public Desk()
        {
            this.Occupied = false;
        }
    }
}
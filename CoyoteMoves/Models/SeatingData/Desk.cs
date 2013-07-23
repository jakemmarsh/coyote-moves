using CoyoteMoves.Models.EmployeeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.SeatingData
{
    public class Desk
    {
        /* we're gonna need to validate against desk numbers
        that already exists for when we make new desks. 
        the front end should do this! */
        public string DeskNumber { get; set; }
        public Location Location { get; set; }
        public Employee CurrentTenant { get; set; }

        public Desk(int FloorNumber, string DeskId)
        {
            this.CurrentTenant = null;
            this.Location = new Location(FloorNumber);
            this.DeskNumber = DeskId;
        }

        public Desk(int FloorNumber, string DeskId, Employee NewGuy)
        {
            this.CurrentTenant = NewGuy;
            this.Location = new Location(FloorNumber);
            this.DeskNumber = DeskId;
        }
    }
}
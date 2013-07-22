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
        public CoordinatePoint TopLeft { get; set; }
        public CoordinatePoint BottomRight { get; set; }
        public int Floor { get; set; }
        public Employee CurrentTenant { get; set; }

        public Desk(int FloorNumber, string DeskId)
        {
            this.CurrentTenant = null;
            this.TopLeft = new CoordinatePoint();
            this.BottomRight = new CoordinatePoint();
            this.DeskNumber = DeskId;
            this.Floor = FloorNumber;
        }

        public Desk(int FloorNumber, string DeskId, Employee NewGuy)
        {
            this.CurrentTenant = NewGuy;
            this.TopLeft = new CoordinatePoint();
            this.BottomRight = new CoordinatePoint();
            this.DeskNumber = DeskId;
            this.Floor = FloorNumber;
        }
    }
}
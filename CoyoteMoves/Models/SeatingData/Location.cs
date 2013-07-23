using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.SeatingData
{
    public class Location
    {
        public int Floor { get; set; }
        public CoordinatePoint TopLeft { get; set; }
        public CoordinatePoint BottomRight { get; set; }

        public Location() 
        {
            this.Floor = -1;
            this.TopLeft = new CoordinatePoint();
            this.BottomRight = new CoordinatePoint();
        }

        public Location(int FloorNumber)
        {
            this.Floor = FloorNumber;
            this.TopLeft = new CoordinatePoint();
            this.BottomRight = new CoordinatePoint();
        }

        public Location(int FloorNumber, CoordinatePoint TopLeftCorner, CoordinatePoint BottomRightCorner)
        {
            this.Floor = FloorNumber;
            this.TopLeft = TopLeftCorner;
            this.BottomRight = BottomRightCorner;
        }

        public bool IsEqualTo(Location other)
        {
            return (this.Floor == other.Floor) && this.TopLeft.IsEqualTo(other.TopLeft) && this.BottomRight.IsEqualTo(other.BottomRight);
        }

    }
}
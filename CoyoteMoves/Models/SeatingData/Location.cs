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
        public double Orientation { get; set; }

        public Location() 
        {
            this.Floor = -1;
            this.TopLeft = new CoordinatePoint();
        }

        public Location(int FloorNumber)
        {
            this.Floor = FloorNumber;
            this.TopLeft = new CoordinatePoint();
            this.Orientation = 0.0;
        }

        public Location(int FloorNumber, CoordinatePoint TopLeftCorner, double angle)
        {
            this.Floor = FloorNumber;
            this.TopLeft = TopLeftCorner;
            this.Orientation = angle;
        }

        public bool IsEqualTo(Location other)
        {
            return (this.Floor == other.Floor) && this.TopLeft.IsEqualTo(other.TopLeft);
        }

    }
}
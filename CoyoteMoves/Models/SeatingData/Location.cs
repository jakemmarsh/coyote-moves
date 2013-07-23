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
        public CoordinatePoint TopRight { get; set; }
        public CoordinatePoint BottomRight { get; set; }
        public CoordinatePoint BottomLeft { get; set; }

        public Location() 
        {
            this.Floor = -1;
            this.TopLeft = new CoordinatePoint();
            this.TopRight = new CoordinatePoint();
            this.BottomRight = new CoordinatePoint();
            this.BottomLeft = new CoordinatePoint();
        }

        public Location(int FloorNumber)
        {
            this.Floor = FloorNumber;
            this.TopLeft = new CoordinatePoint();
            this.TopRight = new CoordinatePoint();
            this.BottomRight = new CoordinatePoint(); 
            this.BottomLeft = new CoordinatePoint();
        }

        public Location(int FloorNumber, CoordinatePoint TopLeftCorner, CoordinatePoint TopRightCorner, CoordinatePoint BottomRightCorner, CoordinatePoint BottomLeftCorner)
        {
            this.Floor = FloorNumber;
            this.TopLeft = TopLeftCorner;
            this.TopRight = TopRightCorner;
            this.BottomRight = BottomRightCorner;
            this.BottomLeft = BottomLeftCorner;
        }

        public bool IsEqualTo(Location other)
        {
            return (this.Floor == other.Floor) && this.TopLeft.IsEqualTo(other.TopLeft) && this.BottomRight.IsEqualTo(other.BottomRight)
                && this.TopRight.IsEqualTo(other.TopRight) && this.BottomLeft.IsEqualTo(other.BottomLeft);
        }

    }
}
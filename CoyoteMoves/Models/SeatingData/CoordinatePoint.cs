using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.SeatingData
{
    public class CoordinatePoint
    {
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }

        public CoordinatePoint(double x, double y)
        {
            this.XCoordinate = x;
            this.YCoordinate = y;
        }

        public CoordinatePoint()
        {
            this.XCoordinate = 0.0;
            this.YCoordinate = 0.0;
        }

        public bool IsEqualTo(CoordinatePoint other)
        {
            return (this.XCoordinate == other.XCoordinate) && (this.YCoordinate == other.YCoordinate);
        }
    }
}
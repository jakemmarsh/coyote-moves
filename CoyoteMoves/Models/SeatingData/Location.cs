using CoyoteMoves.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.SeatingData
{
    public class Location
    {
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public Floor Floot { get; set; }
        public Conference Conference { get; set; }

        public Location()
        {
            this.XCoordinate = 0.0;
            this.YCoordinate = 0.0;
        }
    }
}
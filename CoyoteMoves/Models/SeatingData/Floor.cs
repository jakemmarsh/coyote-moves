using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.SeatingData
{
    public class Floor
    {
        public int FloorNumber { get; set; }
        public List<Desk> Desks { get; set; }

        public Floor(){}

        public Floor(int floor, List<Desk> desks)
        {
            this.FloorNumber = floor;
            this.Desks = desks;
        }

    }
}
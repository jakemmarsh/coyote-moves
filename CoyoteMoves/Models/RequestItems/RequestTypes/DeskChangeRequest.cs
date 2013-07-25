using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class DeskChangeRequest
    {
        public DeskChangeInfo CurrentDeskInfo { get; set; }
        public DeskChangeInfo FutureDeskInfo { get; set; }

        public class DeskChangeInfo
        {
            public int DeskNumber { get; set; }
            public string Office { get; set; }
        }

    }
}
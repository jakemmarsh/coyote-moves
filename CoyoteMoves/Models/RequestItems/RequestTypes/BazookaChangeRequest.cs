using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class BazookaChangeRequest
    {
        public BazookaChangeInfo CurrentBazookaInfo { get; set; }
        public BazookaChangeInfo FutureBazookaInfo { get; set; }

        //security item rights for current should be empty or null
        public class BazookaChangeInfo
        {
            public string JobTitle { get; set; }
            public string Department { get; set; }
            public string Group { get; set; }
            public string ManagerName { get; set; }
            public string JobTemplate { get; set; }
            public string SecurityItemRights { get; set; }
        }

    }
}
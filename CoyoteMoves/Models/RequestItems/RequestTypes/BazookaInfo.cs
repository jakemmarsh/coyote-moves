using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class BazookaInfo
    {
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string Group { get; set; }
        public int ManagerID { get; set; }
        public string JobTemplate { get; set; }
        public string SecurityItemRights { get; set; }
    }
}
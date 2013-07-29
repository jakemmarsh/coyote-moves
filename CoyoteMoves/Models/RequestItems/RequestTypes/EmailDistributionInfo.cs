using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class EmailDistributionInfo
    {
        public List<string> GroupsToAddPersonTo { get; set; }
        public List<string> GroupsToRemovePersonFrom { get; set; }
    }
}
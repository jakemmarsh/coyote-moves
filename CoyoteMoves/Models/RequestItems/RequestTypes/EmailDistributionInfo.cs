using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class EmailDistributionInfo
    {
        public string GroupsToBeAddedTo { get; set; }
        public string GroupsToBeRemovedFrom { get; set; }

        public EmailDistributionInfo()
        {
            GroupsToBeAddedTo = "";
            GroupsToBeRemovedFrom = "";
        }
    }
}
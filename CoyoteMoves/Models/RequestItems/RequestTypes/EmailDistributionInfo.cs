using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class EmailDistributionInfo
    {
        public List<string> GroupsToBeAddedTo { get; set; }
        public List<string> GroupsToBeRemovedFrom { get; set; }

        public EmailDistributionInfo()
        {
            this.GroupsToBeAddedTo = new List<string>();
            this.GroupsToBeRemovedFrom = new List<string>();
        }
    }
}
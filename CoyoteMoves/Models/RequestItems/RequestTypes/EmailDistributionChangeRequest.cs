using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class EmailDistributionChangeRequest
    {
        //not sure how exactly we plan on doing this, but a list of strings seems like a good way...
        public List<string> GroupsToBeAddedTo { get; set; }
        public List<string> GroupsToBeRemovedFrom { get; set; }

    }
}
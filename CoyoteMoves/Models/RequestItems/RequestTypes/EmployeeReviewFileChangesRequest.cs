using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class EmployeeReviewFileChangesRequest
    {
        //i don't really know what information would go in here, so these might need to be redone
        public List<string> ThingsToBeAddedTo { get; set; }
        public List<string> ThingsToBeRemovedFrom { get; set; }

    }
}
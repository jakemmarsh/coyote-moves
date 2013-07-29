using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class EmployeeReviewFileInfo
    {
        //need to change names of these
        public List<string> ThingsToBeAddedTo { get; set; }
        public List<string> ThingsToBeRemovedFrom { get; set; }
    }
}
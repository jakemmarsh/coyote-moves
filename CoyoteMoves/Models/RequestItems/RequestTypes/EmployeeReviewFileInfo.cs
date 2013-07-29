using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class EmployeeReviewFileInfo
    {
        //i don't really know what information would go in here, so these might need to be redone
        public List<string> FilesToBeAddedTo { get; set; }
        public List<string> FilesToBeRemovedFrom { get; set; }
    }
}
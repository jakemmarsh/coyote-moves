using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class EmployeeReviewFileInfo
    {
        public string FilesToBeAddedTo { get; set; }
        public string FilesToBeRemovedFrom { get; set; }

        //REMOVE DEFAULTS IDIOT
        public EmployeeReviewFileInfo()
        {
            FilesToBeAddedTo = "this";
            FilesToBeRemovedFrom = "sucks";
        }
    }
}
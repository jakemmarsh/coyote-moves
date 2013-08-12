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

        public EmployeeReviewFileInfo()
        {
            FilesToBeAddedTo = " ";
            FilesToBeRemovedFrom = " ";
        }
    }
}
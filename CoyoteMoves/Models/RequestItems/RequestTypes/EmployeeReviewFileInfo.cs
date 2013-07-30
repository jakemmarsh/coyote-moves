using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class EmployeeReviewFileInfo
    {
        public List<string> FilesToBeAddedTo { get; set; }
        public List<string> FilesToBeRemovedFrom { get; set; }

        public EmployeeReviewFileInfo()
        {
            this.FilesToBeAddedTo = new List<string>();
            this.FilesToBeRemovedFrom = new List<string>();
        }
    }
}
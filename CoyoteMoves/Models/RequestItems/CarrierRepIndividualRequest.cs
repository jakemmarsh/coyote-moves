using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems
{
    public class CarrierRepIndividualRequest : IndividualRequest
    {
        public List<string> EmployeeReviewFilesToBeAddedTo { get; set; }
        public List<string> EmployeeReviewFilesToBeRemovedFrom { get; set; }
    }
}
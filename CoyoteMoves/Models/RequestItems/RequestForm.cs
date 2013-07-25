using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoyoteMoves.Models.RequestItems.RequestTypes;

namespace CoyoteMoves.Models.RequestItems
{
    public class RequestForm
    {
        /// <summary>
        /// So this is the class that will deal with 'storing' all the data from the json passed from the frontend
        /// </summary>

        public string EmployeeName { get; set; }
        public BazookaChangeRequest BazookaChanges { get; set; }
        public UltiproChangeRequest UltiproChanges { get; set; }
        public DeskChangeRequest DeskChanges { get; set; }
        public PhoneChangeRequest PhoneChanges { get; set; }
        public EmailDistributionChangeRequest EmailDistChange { get; set; }
        public CarrierRepChangeRequest CarrierRepChanges { get; set; }

    }
}
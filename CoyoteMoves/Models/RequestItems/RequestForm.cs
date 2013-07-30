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
        /// just has a bunch of request types (based off of pdf we got from Mario)
        /// </summary>

        public int EmployeeId { get; set; }
        public CoyoteMovesFormEmployeeInfo Current { get; set; }
        public CoyoteMovesFormEmployeeInfo Future { get; set; }
        public List<string> EmailGroupsToBeAddedTo { get; set; }
        public List<string> EmailGroupsToBeRemovedFrom { get; set; }
        public List<string> FilesToBeAddedTo { get; set; }
        public List<string> FilesToBeRemovedFrom { get; set; }

        public RequestForm(int EmployeeID)
        {
            this.EmployeeId = EmployeeID;
            this.Current = new CoyoteMovesFormEmployeeInfo();
            this.Future = new CoyoteMovesFormEmployeeInfo();
            this.EmailGroupsToBeAddedTo = new List<string>();
            this.EmailGroupsToBeRemovedFrom = new List<string>();
            this.FilesToBeAddedTo = new List<string>();
            this.FilesToBeRemovedFrom = new List<string>();
        }

    }
}
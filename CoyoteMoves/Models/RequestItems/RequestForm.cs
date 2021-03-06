﻿using System;
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

        public Guid UniqueId { get; set; }
        public int EmployeeId { get; set; }
        public CoyoteMovesFormEmployeeInfo Current { get; set; }
        public CoyoteMovesFormEmployeeInfo Future { get; set; }
        public EmailDistributionInfo EmailInfo { get; set; }
        public EmployeeReviewFileInfo ReviewInfo { get; set; }
        public int CreatedByID { get; set; }

        public RequestForm()
        {
            UniqueId = Guid.NewGuid();
        }

        public RequestForm(int EmployeeID)
        {
            this.EmployeeId = EmployeeID;
            this.Current = new CoyoteMovesFormEmployeeInfo();
            this.Future = new CoyoteMovesFormEmployeeInfo();
            this.EmailInfo = new EmailDistributionInfo();
            this.ReviewInfo = new EmployeeReviewFileInfo();
            UniqueId = Guid.NewGuid();
        }

    }
}
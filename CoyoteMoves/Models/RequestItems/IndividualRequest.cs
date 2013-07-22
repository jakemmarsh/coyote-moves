using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Models.SeatingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems
{
    public class IndividualRequest
    {
        public Employee CurrentEmployee { get; set; }
        public Desk CurrentDesk { get; set; }
        public RequestFutureProperties FutureDetails { get; set; }
        public DateTime DateToOccurOn { get; set; }
        public List<string> EmailListsToAddTo { get; set; }
        public List<string> EmailListsToBeRemovedFrom { get; set; }
        public string UltiProComment { get; set; }
        public string SecurityItemRights { get; set; }
    }
}
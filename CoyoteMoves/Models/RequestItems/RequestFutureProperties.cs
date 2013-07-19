using CoyoteMoves.Models.EmployeeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems
{
    public class RequestFutureProperties
    {
        public string FutureJobTitles { get; set; }
        public string FutureDepartment { get; set; }
        public string FutureGroup { get; set; }
        public Employee FutureManager { get; set; }
        public string FutureTemplate { get; set; }
        public string FutureSecurityItemRights { get; set; }
        public string FutureDeskNumber { get; set; }
        public string FutureOffice { get; set; }
    }
}
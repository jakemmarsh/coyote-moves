using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class UltiproInfo
    {
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string Supervisor { get; set; }
        public string Other { get; set; }
    }
}
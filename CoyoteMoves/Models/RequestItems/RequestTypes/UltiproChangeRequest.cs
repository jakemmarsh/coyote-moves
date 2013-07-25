using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class UltiproChangeRequest
    {
        public UltiproChangeInfo CurrentUltiproInfo { get; set; }
        public UltiproChangeInfo FutureUltiproInfo { get; set; }

        //current for 'other' should always be empty or null or something
        public class UltiproChangeInfo
        {
            public string JobTitle { get; set; }
            public string Department { get; set; }
            public string Supervisor { get; set; }
            public string Other { get; set; }
        }

    }
}
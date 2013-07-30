using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class CoyoteMovesFormEmployeeInfo
    {

        public BazookaInfo BazookaInfo { get; set; }
        public DeskInfo DeskInfo { get; set; }
        public PhoneInfo PhoneInfo { get; set; }
        public UltiproInfo UltiproInfo { get; set; }
        
    }
}
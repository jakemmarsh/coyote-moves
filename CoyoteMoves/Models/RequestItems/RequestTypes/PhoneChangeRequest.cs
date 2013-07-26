using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class PhoneChangeRequest
    {
        public PhoneChangeInfo CurrentPhoneInfo { get; set; }
        public PhoneChangeInfo FuturePhoneInfo { get; set; }

        //need to add more stuff here later with cisco thingy?
        public class PhoneChangeInfo
        {
            public string PhoneNumber { get; set; }
        }

    }
}
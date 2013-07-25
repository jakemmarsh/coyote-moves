using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models.RequestItems.RequestTypes
{
    public class CarrierRepChangeRequest
    {
        public CarrierRepChange CurrentCarrierRepInfo { get; set; }
        public CarrierRepChange FutureCarrierRepInfo { get; set; }

        public class CarrierRepChange
        {
            //idk what goes here
        }
    }
}
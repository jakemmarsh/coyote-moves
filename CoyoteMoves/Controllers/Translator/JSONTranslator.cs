using CoyoteMoves.Models.SeatingData;
using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Models.RequestItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CoyoteMoves.Controllers
{
    public class JSONTranslator
    {
        public JSONTranslator()
        {

        }

        /// <summary>
        /// Given a floor, returns a string in json format holding the same information as the floor structure
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public string GetJSONFromFloor(Floor floor)
        {
            return JsonConvert.SerializeObject(floor, Formatting.Indented);
        }

        /// <summary>
        /// Given a string in json format, turn that into a floor structure
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public Floor GetFloorFromJSON(string jsonString)
        {
            return JsonConvert.DeserializeObject<Floor>(jsonString);
        }

    }
}
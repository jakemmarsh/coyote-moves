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
        /// Given a list of desks, return a json string corresponding to that information
        /// </summary>
        /// <param name="FloorPlan"></param>
        /// <returns></returns>
        public string GetJSONStringFromFloorPlan(List<Desk> FloorPlan)
        {
            return JsonConvert.SerializeObject(FloorPlan, Formatting.Indented);
        }

        /// <summary>
        /// Get information from a json string and convert it into a list of desk objects
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public List<Desk> GetFloorPlanFromJSONString(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<Desk>>(jsonString);
        }

    }
}
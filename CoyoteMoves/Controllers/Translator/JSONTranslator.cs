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
        /// Given a list of desks, return a string in json format that contains the info on that list
        /// </summary>
        /// <param name="desks"></param>
        /// <returns></returns>
        public static string GetJSONFromListOfDesks(List<Desk> desks)
        {
            return JsonConvert.SerializeObject(desks, Formatting.Indented);
        }

        /// <summary>
        /// Given a string in json format, create a list of desks with the information from the string
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static List<Desk> GetListOfDesksFromJSON(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<Desk>>(jsonString);
        }
    }
}
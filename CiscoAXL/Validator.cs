using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CiscoAXL
{
    class Validator
    {
        /// <summary>
        /// Check if dnorpattern input is valid
        /// </summary>
        /// <param name="dnorpattern"></param>
        /// <returns></returns>
        public bool checkDNORPATTERN(string dnorpattern) 
        {
            Match matcher = Regex.Match(dnorpattern, "^[]0-9*#X[^-]{1,50}$");

            if (matcher.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

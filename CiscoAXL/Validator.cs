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
        /// Check if dnorpattern input is valid. should be handled at database level too.
        /// </summary>
        /// <param name="dnorpattern"></param>
        /// <returns>True if input matches the pattern, False otherwise</returns>
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
        /// <summary>
        /// TODO: check if fkdevice is valid. 
        /// </summary>
        /// <returns>True if input matches the pattern, False otherwise</returns>
        public bool checkFKDEVICE()
        {
            return true;
        }
    }
}

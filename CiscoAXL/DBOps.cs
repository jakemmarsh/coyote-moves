using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoAXL
{
    class DBOps
    {
        Validator test;
        public DBOps()
        {
            test = new Validator();
        }

        /// <summary>
        /// Get display information (name would be a device mac address, description, and fkdevice is a pointer to a the primary ID of a device
        /// ** TODO HANDLE MULTIPLE PHYSICAL DEVICE EXCEPTION
        /// </summary>
        /// <param name="service"></param>
        public void getDisplayInfo(AXLAPIService service, string dnorpattern)
        {
            if (!test.checkDNORPATTERN(dnorpattern))
            {
                throw new Exception("Invalid pattern");
            }
            String sql = "SELECT dnm.fkdevice, d.name, d.description " +
                         "FROM devicenumplanmap AS dnm " +
                         "INNER JOIN numplan AS np " +
                         "ON dnm.fknumplan=np.pkid " +
                         "INNER JOIN device AS d " +
                         "ON dnm.fkdevice=d.pkid " +
                         "WHERE np.dnorpattern='" + dnorpattern + 
                         "'" + "AND dnm.numplanindex='1' AND d.tkmodel IN (404,436,30016)";

            ExecuteSQLQueryReq req = new ExecuteSQLQueryReq();
            req.sql = sql;
            ExecuteSQLQueryRes res = service.executeSQLQuery(req);
            int numRows = res.@return.Length;

            string fkdevice = res.@return.ElementAt(0).ToString();

            foreach (var element in (System.Xml.XmlNode[])(res.@return[0]))
            {
                Console.WriteLine(element.ToString());
            }
            
            Console.WriteLine(fkdevice[0].ToString());

            /*int numRows = res.@return.Length;
            List<String> fkdevice = new List<string>();
            fkdevice.Add();*/
            
        }
    }
}

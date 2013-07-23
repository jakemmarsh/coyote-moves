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
        /// <param name="service">pointer to the AXL API WebService</param>
        /// <param name="dnorpattern">extension number, input comes from active directory</param>
        private List<string> getUserDevices(AXLAPIService service, string dnorpattern)
        {
            if (!test.checkDNORPATTERN(dnorpattern))
            {
                throw new Exception("Invalid pattern");
            }
            //SQL string: numplan=1 makes sure we are selecting devices with "this" dnorpattern as first line association, numplan= 2-6 could be addedd or removed
            //tkmodel IN (404,436,30016) makes sure that we will not update unwanted devices. (UDP/7965/CIPC) respectivly 
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
            //All adding and removing must happen for all devices associated with an enduser. Note that if a user has multiple physical devices then an exception must be handled. Remember Cisco considers softphone as a device and this is ok.
            //fkdevice is a foreign key of a device, this will be primary key in the device table.
            List<string> fkdevice = new List<string>();
            for (int i = 0; i < numRows; i++)
            {
                foreach (var element in (System.Xml.XmlNode[])(res.@return[i]))
                {
                    
                    if(element.Name.Equals("fkdevice"))
                    {
                        fkdevice.Add(element.InnerText);
                    }
                }
            }

            return fkdevice;                      
        }
        public List<PhoneDisplay> getDisplayInfo(AXLAPIService service, string dnorpattern)
        {
            List<string> fkdevice = getUserDevices(service, dnorpattern);
            String fkdevicelist = fkdeviceListToString(fkdevice);
            //SQL query to get the display information for a user as it appears on the phone infront of them. numplanindex = index of line, dnorpattern = extension number, display = display text eg.(Laith Abbas - 6340)
          String  sql = "SELECT np.dnorpattern, dnm.numplanindex, dnm.display " +
                  "FROM numplan AS np " +
                  "INNER JOIN devicenumplanmap AS dnm " +
                  "ON np.pkid=dnm.fknumplan " +
                  "INNER JOIN enduserdevicemap AS eudm " +
                  "ON eudm.fkdevice=dnm.fkdevice " +
                  "WHERE eudm.fkdevice IN " + fkdevicelist + " AND eudm.tkuserassociation='1' " +
                  "ORDER BY dnm.numplanindex";
          ExecuteSQLQueryReq req = new ExecuteSQLQueryReq();
          req.sql = sql;

          ExecuteSQLQueryRes response = service.executeSQLQuery(req);
          int numRows = response.@return.Length;

          
          List<PhoneDisplay> phoneDisplayList = new List<PhoneDisplay>();
          for (int i = 0; i < numRows; i++)
          {
              PhoneDisplay phoneDisplay = new PhoneDisplay();
              foreach (var element in (System.Xml.XmlNode[])(response.@return[i]))
              {
                  if (element.Name.Equals("numplanindex"))
                      phoneDisplay.numplanindex = element.InnerText;
                  else if (element.Name.Equals("dnorpattern"))
                      phoneDisplay.dnorpattern = element.InnerText;
                  else if (element.Name.Equals("display"))
                      phoneDisplay.display = element.InnerText;  
              }
              phoneDisplayList.Add(phoneDisplay);
          }
          return phoneDisplayList;
       }
        /// <summary>
        /// //Convert the List of fkdevice into a comma seperated string for SQL input using 'IN'
        /// </summary>
        /// <returns>Comma seperated string of fkdevices</returns>
        private String fkdeviceListToString(List<string> fkdevice)
        {
            
            ///TODO: Create Regex validor for these inputs incase of code injections. Should be validated by teh database also.
            String fkdevicelist = "(";
            for (int i = 0; i < fkdevice.Count; i++)
            {
                fkdevicelist += "'";
                fkdevicelist += fkdevice[i];
                if (i == fkdevice.Count() - 1)
                {
                    fkdevicelist += "'";
                }
                else
                {
                    fkdevicelist += "',";
                }
            }
            fkdevicelist += ")";

            return fkdevicelist;
        }
    }
}

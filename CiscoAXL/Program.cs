using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Web.Services.Protocols;
namespace CiscoAXL
{
    /// <summary>
    /// Prototyping AXL Web Service
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string ccmIP = "10.3.221.1";
            string user = "axluser";
            string password = "C0y0tePWR!";

            AXLAPIService service = new AXLAPIService(ccmIP, user, password);
            service.ConnectionGroupName = "CUCM:DB ver=9.1";

           // callGetPhone(service);
            //callUpdateLine(service);
            string ext = "6340";
            //callGetLine(service, ext);
           // callExecuteQuery(service);
           // callUpdatePhone(service);
            //Validator test = new Validator();
            DBOps ops = new DBOps();
            ops.getDisplayInfo(service, ext);
        

        }
        /// <summary>
        /// Gets details about a device. A device could be a 7965/7962 physical device or it could be a soft phone or a udp profile. 
        /// </summary>
        /// <param name="service"></param>
        public static void callGetPhone(AXLAPIService service)
        {
            GetPhoneReq req = new GetPhoneReq();
            req.ItemElementName = ItemChoiceType136.name;
            req.Item = "SEP08CC683120A3";

            req.returnedTags = new RPhone();
            req.returnedTags.name = "";
            req.returnedTags.description = "";
            req.returnedTags.lines = new RPhoneLines();
            req.returnedTags.lines.Items = new RPhoneLine[1];
            RPhoneLine returnedLineTags = new RPhoneLine();
            returnedLineTags.dirn = new RDirn();
            returnedLineTags.dirn.pattern = "";

            returnedLineTags.dirn.routePartitionName = null;
            returnedLineTags.index = null;
            req.returnedTags.lines.Items.SetValue(returnedLineTags, 0);
            GetPhoneRes getResponse = service.getPhone(req);

            Console.WriteLine("Successfully retrieving  Phone using  GetPhoneRequest" + getResponse.@return);
            GetPhoneResReturn phoneResReturn = getResponse.@return;
            Console.WriteLine("phone name" + phoneResReturn.phone.name);
            Console.WriteLine("phone description" + phoneResReturn.phone.description);
            int noOfLines = phoneResReturn.phone.lines.Items.Length;
            Console.WriteLine("no of lines associated  " + noOfLines);
            int i = 0;
            while (i < noOfLines)
            {
                RPhoneLine xyz = (RPhoneLine)phoneResReturn.phone.lines.Items[i];
                Console.WriteLine("Line ----" + i);
                Console.WriteLine("index of the line " + xyz.index);
                

                Console.WriteLine("Pattern" + xyz.dirn.pattern);
                Console.WriteLine("RoutePartitionName" + xyz.dirn.routePartitionName);
                i++;
            }
            
        }

        public static void callUpdateLine(AXLAPIService service)
        {
           

        }
        /// <summary>
        /// Get Line method, queries the database "numplan" table, grabbing information related to a particular line. Please see the data dictionary for more info
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ext">Extension number to query informationa bout</param>
        public static void callGetLine(AXLAPIService service, string ext)
        {
            GetLineReq req = new GetLineReq();
            req.ItemsElementName = new ItemsChoiceType55[2];
            req.ItemsElementName[0] = ItemsChoiceType55.pattern;
            req.ItemsElementName[1] = ItemsChoiceType55.routePartitionName;
            req.Items = new object[2];
            req.Items[0] = ext;
            string routeName = "Cluster DN Presence Allowed";
            XFkType routePart = new XFkType();
            routePart.Value = routeName;
            req.Items[1] = routePart;
            req.returnedTags = new RLine();
            req.returnedTags.pattern = "";
            req.returnedTags.description = "";
            req.returnedTags.patternPrecedence = "";
            GetLineRes res = service.getLine(req);
            GetLineResReturn ret = res.@return;

        }
        /// <summary>
        /// Passing SQL queries to the server using the AXL web service. This could also be done using telnet and it directly access the database. See Data Dictionary
        /// </summary>
        /// <param name="service"></param>
        public static void callExecuteQuery(AXLAPIService service)
        {
            ExecuteSQLQueryReq req = new ExecuteSQLQueryReq();
            string sql = "select * from numplan where dnorpattern='5850'";
            req.sql = sql;
            ExecuteSQLQueryRes res = service.executeSQLQuery(req);
            
        }
        /// <summary>
        /// NOT WORKING PROPERLY
        /// This function is intended to update the lines on a certain device. It access fields from the database tables: device, devicenumplanmap and numplan. It does not allow me to add a line to an existing device.
        /// </summary>
        /// <param name="service"></param>
        public static void callUpdatePhone(AXLAPIService service)
        {
            UpdatePhoneReq req = new UpdatePhoneReq();
            req.ItemElementName = ItemChoiceType34.name;
            req.Item = "SEP08CC683120A3";
            req.lines = new UpdatePhoneReqLines();
            req.lines.Items = new XPhoneLine[1];
            XPhoneLine line = new XPhoneLine();
            line.index = "3";
            line.display = "Laith Abbas - 6340";
            line.displayAscii = "Laith Abbas - 6340";
            line.asciiLabel = "Laith Abbas - 6340";
            line.label = "Laith Abbas - 6340";
            line.dirn = new XDirn();
            line.dirn.pattern = "6340";
            req.lines.Items.SetValue(line, 0);
            line.e164Mask = "773365XXXX";
            line.maxNumCalls = "24";
            line.busyTrigger = "2";

            XFkType xfktype = new XFkType();
            xfktype.Value = "Standard 7965 SCCP w/6 lines";
            req.phoneTemplateName = xfktype;
            xfktype = new XFkType();
            xfktype.Value = "Cisco 7965 - Standard SCCP Non-Secure Profile";
            req.securityProfileName = xfktype;

            xfktype = new XFkType();
            xfktype.Value = "Chicago GX Phones";
            req.devicePoolName = xfktype;
            StandardResponse res = service.updatePhone(req);
        }
    
    }
}

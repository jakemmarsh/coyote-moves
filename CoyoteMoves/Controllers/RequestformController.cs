using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoyoteMoves.Models.RequestItems;
using CoyoteMoves.Data_Access;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using CoyoteMoves.Models.SeatingData;
using CoyoteMoves.Models.EmployeeData;
using CoyoteMoves.Emailer.Models;
using CoyoteMoves.Emailer;
using System.Collections.ObjectModel;

namespace CoyoteMoves.Controllers
{
    public class RequestFormController : ApiController
    {
        /*
         * Frontend will send us json (or just the object?) with the future (and current?) coyote moves form information
         * This controller is for receiving it and turning it into data objects to work with easier
         * */
        // POST api/RequestForm/ReceiveFormChangeRequest
        public HttpResponseMessage SendChangeRequest(JObject json)
        {
            RequestForm obj = null;
            using(var sr = new StringReader(json.ToString()))
            using(var jr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                obj = (RequestForm)js.Deserialize<RequestForm>(jr);
            }
            //turn the json into data objects

            //EmailSender emailer = new EmailSender();
            //emailer.sendMovesRequestHR(obj);
            //emailer.sendMovesRequestSD(obj);

            Collection<string> to = new Collection<string>();
            //to.Add("jason.dibabbo@coyote.com");
            to.Add("kevin.jasieniecki@coyote.com");
            EmailSender emailer = new EmailSender("Testes", to, "coyotemoves@coyote.com", "Testing.", HttpContext.Current.Server.MapPath("CoyoteMoves/CoyoteMovesTemplate.pdf"));
            //send the email (with the old and changed info) to service desk and HR to approve of the changes

            RequestFormDB formDB = new RequestFormDB();
            formDB.StoreRequestFormInDatabaseAsPending(obj);
            //add it to the queue of "unapproved", log the attempt to change?

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        //GET api/RequestForm/GetIDFromName
        public int GetIDFromName(string name)
        {
            EmployeeDB edb = new EmployeeDB();
            return edb.GetIdFromName(name);
        }

        // GET api/RequestForm/GetAllGroups
        public List<string> GetAllGroups()
        {
            RequestDataDB dbaccess = new RequestDataDB();
            return dbaccess.GetAllGroups();
        }

        // GET api/RequestForm/GetAllDepartments
        public List<string> GetAllDepartments()
        {
            RequestDataDB dbaccess = new RequestDataDB();
            return dbaccess.GetAllDepartments();
        }

        // GET api/RequestForm/GetAllJobTitles
        public List<string> GetAllJobTitles()
        {
            RequestDataDB dbaccess = new RequestDataDB();
            return dbaccess.GetAllJobTitles();
        }


        /*
        * INB4: This should probably go in another class, just writing it down so I don't forget
        * Once the changes have been requested and have been approved at HR and service desk,
        * we'll then have to update the database(s)
        * */
        public void UpdateDataSourcesWithApprovedChanges()
        {
            //idk what it should take in, or what it should return...

            //remove this change request from the "unapproved" queue and (move it to the "approved" queue? or just delete it?)

            //send the updated info to the proper data source (have to talk to Bazooka, Active Directory, Cisco, (Ultipro?)
            //probably a helper function for updating each source...
        }

    }
}

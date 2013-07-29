using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoyoteMoves.Models.RequestItems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoyoteMoves.Controllers
{
    public class FormController : ApiController
    {

        /*
         * Frontend will send us json (or just the object?) with the future (and current?) coyote moves form information
         * This controller is for receiving it and turning it into data objects to work with easier
         * */
        // POST api/Form/ReceiveFormChangeRequest
        [HttpPost]
        public HttpResponseMessage ReceiveFormChangeRequest(RequestForm changes)
        {
            //turn the json into data objects

            //send the email (with the old and changed info) to service desk and HR to approve of the changes

            //add it to the queue of "unapproved", log the attempt to change?

            return Request.CreateResponse(HttpStatusCode.OK, new RequestForm());
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

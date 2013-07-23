using CoyoteMoves.Models.RequestItems;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace CoyoteMoves.Controllers
{
    public class IndividualRequestController : ApiController
    {
        //
        // POST: /IndividualRequest/

        //can change this parameter to be an individual request type as long as
        //the returned json lines up with each attribute
        [HttpPost, HttpPut]
        public HttpResponseMessage Create(JObject jsonData)
        {
            if (null == jsonData)
            {
                throw new ArgumentNullException("jsonData");
            }

            //Need to take the JSON Object that was passed to me and send it to the front end
            //How the hell do I do this?
            return Request.CreateResponse(HttpStatusCode.OK, new IndividualRequest());
        }


    }
}

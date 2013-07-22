using CoyoteMoves.Models.RequestItems;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace CoyoteMoves.Controllers
{
    public class IndividualRequestController : Controller
    {
        //
        // GET: /IndividualRequest/

        public ActionResult Index()
        {
            return View();
        }

        public HttpResponseMessage Get(IndividualRequest request)
        {
            //Need to take the request I'm given and 
            return null;
        }

        [HttpGet]
        public HttpResponseMessage MapData(int floor)
        {
            //front end is asking for map data
            //give them the desk data for a floor

            //gonna need to query the database for desks


            return null;
        }

        [HttpPost, HttpPut]
        public HttpRequestMessage Post(JObject jsonData)
        {
            if (null == jsonData)
            {
                throw new ArgumentNullException("jsonData");
            }

            //Need to take the JSON Object that was passed to me and send it to the front end
            //How the hell do I do this?
            return null;
        }


    }
}

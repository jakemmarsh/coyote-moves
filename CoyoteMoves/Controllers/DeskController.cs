using CoyoteMoves.Models.SeatingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoyoteMoves.Controllers
{
    public class DeskController : ApiController
    {
        // GET api/desk
        public HttpResponseMessage Get(int floor)
        {
            Desk temp = new Desk(5, "ABC123");
            return Request.CreateResponse(HttpStatusCode.OK, temp);
        }

        //// GET api/desk/5
        //public string Get(int id)
        //{
        //    return "value";
        //}
    }
}

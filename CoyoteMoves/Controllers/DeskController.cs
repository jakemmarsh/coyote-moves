using CoyoteMoves.Data_Access;
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
            DeskDB DeskDataBaseConnection = new DeskDB();
            List<Desk> DeskList = DeskDataBaseConnection.GetDesksFromFloor(floor);
            return Request.CreateResponse(HttpStatusCode.OK, DeskList);
        }

        //// GET api/desk/5
        //public string Get(int id)
        //{
        //    return "value";
        //}
    }
}

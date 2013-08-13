using CoyoteMoves.Data_Access;
using CoyoteMoves.Models.SeatingData;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace CoyoteMoves.Controllers
{
    public class DeskController : ApiController
    {
        private DeskDB DeskDataBaseConnection = new DeskDB();
        // GET api/Desk/GetDesksByFloor/{floorNumber}
        public List<Desk> GetDesksByFloor(int id)
        {
            List<Desk> DeskList = DeskDataBaseConnection.GetDesksFromFloor(id);
            return DeskList;
        }

        public List<Desk> GetDesks()
        {
            return DeskDataBaseConnection.GetAllDesks();
        }

        // POST api/Desk/SetDeskOrientationAndPoint/{deskNumber}
        public HttpResponseMessage SetDeskOrientationAndPoint(JObject json)
        {
            double x = (double)json["x"];
            double y = (double)json["y"];
            double orientation = (double)json["orientation"];
            string deskNumber = json["deskNumber"].ToString();

            if (DeskDataBaseConnection.ChangeDeskPointAndOrientation(deskNumber, x, y, orientation))
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            else
            {
                HttpResponseMessage deskNotFound = new HttpResponseMessage(HttpStatusCode.NotFound);
                deskNotFound.Content = new StringContent("Desk not found.");
                return deskNotFound;
            }
        }
    }
}

using CoyoteMoves.Data_Access;
using CoyoteMoves.Models.SeatingData;
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
        // GET api/Desk/GetDesksByFloor/{floorNumber}
        public List<Desk> GetDesksByFloor(int id)
        {
            DeskDB DeskDataBaseConnection = new DeskDB();
            List<Desk> DeskList = DeskDataBaseConnection.GetDesksFromFloor(id);
            return DeskList;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UtilitiesChat.Models.WS;
using ChatWS.Models;

namespace ChatWS.Controllers
{
    public class RoomController : BaseController
    {
        [HttpPost]
        public Reply List([FromBody] SecurityRequest model)
        {
            Reply oR = new UtilitiesChat.Models.WS.Reply();

            oR.result = 0;
            if (!VerifyToken(model))
            {
                oR.message = "Método no permitido";
                return oR;
            }

            using (ChatDBEntities db = new ChatDBEntities())
            {
                List<ListRoomsResponce> lstRoomsResponce = (from d in db.room
                                                     where d.idState==1
                                                     orderby d.name
                                                     select new ListRoomsResponce { 
                                                         Name = d.name,
                                                         Description=d.description,
                                                         Id=d.id
                                                     }).ToList();
                oR.data = lstRoomsResponce;
            }
            oR.result = 1;

            return oR;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UtilitiesChat.Models.WS;
using ChatWS.Models;

namespace ChatWS.Controllers
{
    public class BaseController : ApiController
    {
        public UserResponse oUserSession;
        protected bool VerifyToken(SecurityRequest model)
        {
            //verificar si existe ese token que se envia
            using (ChatDBEntities db=new ChatDBEntities())
            {
                var oUser = db.user.Where(d => d.access_token == model.AccessToken).FirstOrDefault();

                if(oUser != null)
                {
                    oUserSession = new UserResponse();

                    oUserSession.AccessToken = oUser.access_token;
                    oUserSession.City = oUser.city;
                    oUserSession.Name = oUser.name;
                    oUserSession.Id = oUser.id;

                    return true;
                }
            }

            return false;
        }
    }
}

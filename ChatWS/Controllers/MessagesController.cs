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
    public class MessagesController : BaseController
    {
        [HttpPost]
        public Reply Get(MessagesRequest model)
        {
            Reply oR = new Reply();

            oR.result = 0;
            if (!VerifyToken(model))
            {
                oR.message = "Método no permitido";
                return oR;
            }

            try
            {
                using (ChatDBEntities db = new ChatDBEntities())
                {
                    List<MessagesResponse> lst = (from d in db.message.ToList()
                                                  where d.idState == 1 && d.idRoom == model.IdRoom
                                                  orderby d.date_created descending
                                                  select new MessagesResponse
                                                  {
                                                      Message = d.text,
                                                      Id = d.id,
                                                      IdUser = d.idUser,
                                                      UserName = d.user.name,
                                                      DateCreated = d.date_created,
                                                      TypeMessage = (
                                                                new Func<int>(
                                                                    () =>
                                                                    {
                                                                        try
                                                                        {
                                                                            if (d.idUser == oUserSession.Id)
                                                                                return 1;
                                                                            else
                                                                                return 2;
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            return 2;
                                                                        }
                                                                    }
                                                                    )()
                                                      )
                                                  }).Take(20).ToList();
                    oR.result = 1;
                    oR.data = lst;
                }
            }
            catch (Exception ex)
            {
                oR.message="Ocurrio un error";
            }
            return oR;
        }
    }
}

using ChatWS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UtilitiesChat.Models.WS;

namespace ChatWS.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        public Reply Get()
        {
            Reply oReply = new Reply();
            using (Models.ChatDBEntities db = new Models.ChatDBEntities())
            {
                List<UserViewModel> lst = (from d in db.user
                                          select new UserViewModel
                                          {
                                              Name=d.name,
                                              City=d.city
                                          }).ToList();
                oReply.result = 1;
                oReply.data = lst;
            }
            return oReply;
        }
        [HttpPost]
        public Reply Register([FromBody] Models.Request.User model)
        {
            Reply oReply = new Reply();
            try
            {
                using (Models.ChatDBEntities db = new Models.ChatDBEntities())
                {
                    Models.user oUser = new Models.user();
                    oUser.name = model.Name;
                    oUser.password = model.Password;
                    oUser.email = model.Email;
                    oUser.city = model.City;
                    oUser.date_created = DateTime.Now;
                    oUser.idState = 1;

                    db.user.Add(oUser);
                    db.SaveChanges();

                    oReply.result = 1;
                }

            }
            catch (Exception ex)
            {
                oReply.result = 0;
                oReply.message = "intenta de nuevo más tarde";
                //log
            }
            return oReply;
        }
    }
}

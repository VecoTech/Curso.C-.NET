using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilitiesChat;
using ChatWeb.Business;
using ChatWeb.Models.ViewModels;
using UtilitiesChat.Models.WS;
using Newtonsoft.Json;

namespace ChatWeb.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Login()
        {
            UserAccessViewModel model = new UserAccessViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(UserAccessViewModel model)
        {
            //Valida si algun DataAnnotation ingresado no se cumple
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            AccessRequest oAR = new AccessRequest();
            oAR.Email = model.Email;
            oAR.Password = UtilitiesChat.Tools.Encrypt.GetSHA256(model.Password);

            RequestUtil oRequestUtil = new RequestUtil();
            UtilitiesChat.Models.WS.Reply oReply = oRequestUtil.Execute<AccessRequest>(Constants.Url.ACCESS, "post", oAR);

            UserResponse oUserResponse = JsonConvert.DeserializeObject<UserResponse>(
                JsonConvert.SerializeObject(oReply.data)
                );

            if (oReply.result == 1)
            {
                Session["User"] = oUserResponse;
                return RedirectToAction("Index", "Lobby");
            }

            ViewBag.error = "Datos Incorrectos";
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            ChatWeb.Models.ViewModels.RegisterViewModel model = new ChatWeb.Models.ViewModels.RegisterViewModel();
            return View();
        }
        [HttpPost]
        public ActionResult Register(ChatWeb.Models.ViewModels.RegisterViewModel model)
        {
            //Valida si algun DataAnnotation ingresado no se cumple
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Models.Request.User oUser = new Models.Request.User();
            oUser.Name = model.Name;
            oUser.City = model.City;
            oUser.Email = model.Email;
            oUser.Password = model.Password;

            RequestUtil oRequestUtil = new RequestUtil();
            UtilitiesChat.Models.WS.Reply oReply = oRequestUtil.Execute<Models.Request.User>(Constants.Url.REGISTER, "post", oUser);

            return View();
        }
    }
}
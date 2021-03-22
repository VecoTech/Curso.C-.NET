using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using ChatWS.Models;

namespace ChatWS
{
    public class CounterHub : Hub
    {
        public override Task OnConnected()
        {
            Clients.All.enterUser();//metodo que se crea
            return base.OnConnected();
        }

        public void AddGroup(int idRoom)
        {
            Groups.Add(Context.ConnectionId, idRoom.ToString());
        }
        public void Send(int idRoom, string userName, int idUser, string message, string AccessToken)
        {

            if (VerifyToken(AccessToken))
            {
                string fecha = DateTime.Now.ToString();

                using (ChatDBEntities db = new ChatDBEntities())
                {
                    var oMessage = new message();
                    oMessage.idRoom = idRoom;
                    oMessage.date_created = DateTime.Now;
                    oMessage.idUser = idUser;
                    oMessage.text = message;
                    oMessage.idState = 1;

                    db.message.Add(oMessage);
                    db.SaveChanges();
                }

                //Para no enviar a todos los grupos se comenta y se realiza lo siguiente
                //Clients.All.sendChat(userName, message, fecha, idUser);
                Clients.Group(idRoom.ToString()).sendChat(userName, message, fecha, idUser);
            }
        }

        protected bool VerifyToken(string AccessToken)
        {
            //verificar si existe ese token que se envia
            using (ChatDBEntities db = new ChatDBEntities())
            {
                var oUser = db.user.Where(d => d.access_token == AccessToken).FirstOrDefault();

                if (oUser != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
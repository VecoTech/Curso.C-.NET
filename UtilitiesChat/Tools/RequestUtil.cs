using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using UtilitiesChat.Models.WS;

namespace UtilitiesChat
{
    public class RequestUtil
    {
        public Reply oReply { get; set; }
        //Constructor
        public RequestUtil()
        {
            //Reinicializar
            oReply = new Reply();
        }

        // Metodo invocar Servicio Web
        //Metodo url de nuestro servicio, metodo donde se envia esto(POST), objeto que voy a enviar serializado en json T=objeto
        public Reply Execute<T>(string url, string method, T objectRequest)
        {
            oReply.result = 0;
            string result = "";
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                //Transformar en json el objeto
                string json = JsonConvert.SerializeObject(objectRequest);
                //Solicitud web
                WebRequest request = WebRequest.Create(url);
                request.Method = method;
                request.PreAuthenticate = true;
                request.ContentType = "application/json;charset=utf-8";
                request.Timeout = 60000;//1 minuto

                //Crear el body
                using (var oStreamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    oStreamWriter.Write(json);
                    oStreamWriter.Flush(); //Liberamos el flujo
                }
                //hacer la solicitud
                var oHttpResponse = (HttpWebResponse)request.GetResponse();
                //escribir la respuesta
                using (var oStreanReader = new StreamReader(oHttpResponse.GetResponseStream()))
                {
                    result = oStreanReader.ReadToEnd();// Leer todo lo que tienes
                }

                oReply = JsonConvert.DeserializeObject<Reply>(result);
            }
            catch (TimeoutException ex)
            {
                oReply.message = "Servidor sin respuesta " + ex.Message;

            }
            catch (Exception ex)
            {
                oReply.message = "Ocurrio un Error en Cliente " + ex.Message;
            }
            return oReply;
        }
    }
}

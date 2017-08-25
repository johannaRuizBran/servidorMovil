using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using servidor.Models;
using System.Net.Http;

using System.Web.Script.Services;
using System.Web.Services;

namespace servidor.Controllers
{
    public class usuarioController : Controller
    {
        private usuarioManager usuarioManager;
        public usuarioController()
        {
            usuarioManager = new usuarioManager();
        }

        // insertar a un usuario 
        [HttpPost]
        public JsonResult insertarUsuarioAction(string permisoAdmin, Usuario user)
        {
            return Json(usuarioManager.insertarUsuario(permisoAdmin,user));
        }        

        //obtiene lista de usuarios segun tipo de permisos
        [HttpGet]
        public JsonResult obtenerListaUsuariosAction(string tipoPermiso)
        {
            return Json(usuarioManager.obtenerListaUsuarios(tipoPermiso),
                        JsonRequestBehavior.AllowGet);
        }

        //elimina un usuario de la base
        [HttpPost]
        public JsonResult eliminarUsuarioAction(string nombreUsuario)
        {
            return Json(usuarioManager.eliminarUsuario(nombreUsuario));
        }

        //actualiza la info de un usuario (se cuenta con permisos de administrador)
        [HttpPost]
        public JsonResult actualizarUsuarioAction(string permisoAdmin,string nombreUsuarioOLD, Usuario usuario)
        {
            return Json(usuarioManager.actualizarUsuario(permisoAdmin,nombreUsuarioOLD,usuario));
        }
      
        [HttpGet]
        public JsonResult obtenerUsuarioLoginAction(string nombreU, string contr, Usuario item)
        {
            switch (Request.HttpMethod)
            {
                case "GET":
                    return Json(usuarioManager.obtenerUsuarioLogin(nombreU, contr), JsonRequestBehavior.AllowGet);
            }
            return Json(new { Error = true, Message = "Operación HTTP desconocida" });
        }

        [HttpGet]
        public JsonResult obtenerUsuarioInf(string nombreU, Usuario item)
        {
            switch (Request.HttpMethod)
            {
                case "GET":
                    return Json(usuarioManager.obtenerUsuarioInfo(nombreU), JsonRequestBehavior.AllowGet);
            }
            return Json(new { Error = true, Message = "Operación HTTP desconocida" });
        }


        //nuevo funciones push


        //enviar mensaje
        [HttpPost]
        public JsonResult enviarMensajePushAction(string appID, string senderID, string deviceID, string mensaje)
        {
            return Json(usuarioManager.enviarMensajePush(appID, senderID, deviceID, mensaje));
        }

        //actualizar token
        [HttpPost]
        public JsonResult actualizarTokenPushNotf(string nombreUsuario, string id)
        {
            return Json(usuarioManager.actualizarTokenPushNotf(nombreUsuario, id));
        }

        //obtener el token del usuario (id del telefono especifico)
        [HttpGet]
        public JsonResult obtenerTokenUsuarioAction(string nombreUsuario)
        {
            return Json(usuarioManager.obtenerTokenUsuario(nombreUsuario),
                        JsonRequestBehavior.AllowGet);
        }


    }
}

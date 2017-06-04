using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Routing;

namespace servidor
{
    [EnableCors("*", "*", "*")]
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //      ************* RUTAS USUARIO *************
            

            //var cors = new EnableCorsAttribute("*", "*", "*");
            
            //insertar un usuario 

            routes.MapRoute(
                name: "AccesoUsuariosInsertarUsuario",
                url: "Usuarios/insertarUsuario/{permisoAdmin}",
                defaults: new
                {
                    controller = "usuario",
                    action = "insertarUsuarioAction"
                }
            );

            //obtener lista de usuarios
            routes.MapRoute(
                name: "AccesoUsuariosListaUsuarios",
                url: "Usuarios/obtenerListaUsuarios/{tipoPermiso}",
                defaults: new {
                    controller = "usuario",
                    action = "obtenerListaUsuariosAction"
                }
            );

            //eliminar un usuario

            routes.MapRoute(
                name: "AccesoUsuariosEliminarUsuario",
                url: "Usuarios/eliminarUsuario/{nombreUsuario}",
                defaults: new
                {
                    controller = "usuario",
                    action = "eliminarUsuarioAction"
                }
            );


            //actualizar usuario (usuario tiene permisos de administrador)

            routes.MapRoute(
                name: "AccesoUsuariosActualizarUsuario",
                url: "Usuarios/actualizarUsuario/{permisoAdmin}/{nombreUsuarioOLD}",
                defaults: new
                {
                    controller = "usuario",
                    action = "actualizarUsuarioAction"
                }
            );


            //autentificar usuario login
            routes.MapRoute(
                name: "AccesoUsuarios",
                url: "Usuarios/ObtenerInfo/{nombreU}/{contr}",
                defaults: new
                {
                    controller = "usuario",
                    action = "obtenerUsuarioLoginAction",                   
                }
            );


            //informacion de una persona
            routes.MapRoute(
                name: "AccesoSoloUnUsuario",
                url: "Usuario/ObtenerInfo/{nombreU}",
                defaults: new
                {
                    controller = "usuario",
                    action = "obtenerUsuarioInf",
                }
            );


            /////////////////////////////   REPORTES    /////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////

            //crear reporte
            routes.MapRoute(
                name: "crearReporte",
                url: "Reportes/crearReporte",
                defaults: new
                {
                    controller = "reporte",
                    action = "crearReporte",
                }
            );

            //obtener todos los reportes que ha hecho una persona
            routes.MapRoute(
                name: "listaDeMisreportes",
                url: "Reportes/ObtenerMisReportes/{nombreU}",
                defaults: new
                {
                    controller = "reporte",
                    action = "obtenerReporteUsuario",
                }
            );

            //obtener información de un reporte
            routes.MapRoute(
                name: "informacionReporte",
                url: "Reportes/informacionReporte/{idReporte}",
                defaults: new
                {
                    controller = "reporte",
                    action = "obtenerReporteInformacion",
                }
            );

            //Cancelar Reporte
            routes.MapRoute(
                name: "cancelarReporteUsuario",
                url: "Reportes/Cancelar/{idReporte}",
                defaults: new
                {
                    controller = "reporte",
                    action = "cancelarReporteUsuario",
                }
            );


            //información incompleta
            routes.MapRoute(
                name: "informacionFaltante",
                url: "Reportes/informacionFaltante/{idReporte}",
                defaults: new
                {
                    controller = "informacionFalta",
                    action = "informacionFaltante",
                }
            );

            ///////////////////////////////////////// nuevo ////////////////////

            routes.MapRoute(
                name: "AccesoReportesListaReportesEstado",
                url: "Reporte/obtenerListaReportesEstado/{estadoReporte}",
                defaults: new
                {
                    controller = "reporte",
                    action = "obtenerInfoReportesEstadoAction",
                }
            );
        }
    }
}

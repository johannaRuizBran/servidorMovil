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
            /////////////////////////////   LABORATORIOS    /////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////

            //obtener PCs de un laboratorio por id del reporte
            routes.MapRoute(
                name: "listaPCsDeLaboratorio",
                url: "Computadora/listaPCs/{idReporte}",
                defaults: new
                {
                    controller = "Computadora",
                    action = "obtenerPCsReporte",
                }
            );
            ///1/C-009/Gris/full
            //Modificar detalle reporte
            routes.MapRoute(
                name: "actualizarColorDescripcionPC",
                url: "Computadora/actualizarReportePC/{idReporte}/{idPC}/{color}/{descripcion}",
                defaults: new
                {
                    controller = "Computadora",
                    action = "modificarDetalleReporte",
                }
            );

            /////////////////////////////   Tecnicos    /////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////

            //obtener todos mis reportes pos atender
            routes.MapRoute(
                name: "listaDeMisreportesPendientes",
                url: "Reportes/ObtenerMisReportesPendientes/{nombreU}",
                defaults: new
                {
                    controller = "reporte",
                    action = "obtenerReporteTecnicos",
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

            //obtener informaci�n de un reporte
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
                name: "cambiarEstadoReporte",
                url: "Reportes/cambiarEstado/{idReporte}/{estado}",
                defaults: new
                {
                    controller = "reporte",
                    action = "cambiarEstadoReporte",
                }
            );


            //informaci�n incompleta
            routes.MapRoute(
                name: "informacionFaltante",
                url: "Reportes/informacionFaltante/{idReporte}",
                defaults: new
                {
                    controller = "informacionFalta",
                    action = "informacionFaltante",
                }
            );

            // lista de reportes segun estado del reporte

            routes.MapRoute(
                name: "AccesoReportesListaReportesEstado",
                url: "Reporte/obtenerListaReportesEstado/{estadoReporte}",
                defaults: new
                {
                    controller = "reporte",
                    action = "obtenerInfoReportesEstadoAction",
                }
            );

            // obtener lista de reportes priorizados segun el filtro seleccionado

            routes.MapRoute(
                name: "AccesoReportesListaReportesPriorizadosFiltro",
                url: "Reporte/obtenerListaReportesPriorizadosFiltro/{tipo}",
                defaults: new
                {
                    controller = "reporte",
                    action = "obtenerListaReportesPriorizadosFiltroAction",
                }
            );

            ///////////////////////////   INFORMACION FALTANTE


            // solicita mas informacion de un reporte
            routes.MapRoute(
                name: "AccesoReportesSolicitarInformacion",
                url: "Reporte/reportesSolicitarInformacion/{idReporte}/{infomacion}",
                defaults: new
                {
                    controller = "informacionFalta",
                    action = "solicitarMasInformacionReporteAction",
                }
            );


            // insertar mas infromacion de reporte
            routes.MapRoute(
                name: "AccesoAgregarMasInformacion",
                url: "Reportes/informacionFaltante/agregar/{idReporte}/{infomacion}",
                defaults: new
                {
                    controller = "informacionFalta",
                    action = "agregarMasInformacionFunc",
                }
            );



            // asigna un tecnico a un reporte       
            routes.MapRoute(
                name: "AccesoReporteAsignarTecnico",
                url: "Reporte/asignarTecnico/{idReporte}/{idUsuario}",
                defaults: new
                {
                    controller = "reporte",
                    action = "asignarTecnicoReporteAction",
                }
            );



            //actuliza la prioridad de un reporte
            routes.MapRoute(
                name: "AccesoActualizarPrioridadReporte",
                url: "Reporte/actualizarPrioridad/{idReporte}/{fechaFinalizacion}/{nivelPrioridad}",
                defaults: new
                {
                    controller = "reporte",
                    action = "actualizarPrioridadAction",
                }
            );





            ///////////////////////////////////////////////////
            ///////////////////// FUNCIONES PUSH
            ///////////////////////////////////////////////////

            routes.MapRoute(
                name: "ActualizartokenIdPush",
                url: "Usuarios/actualizarTokenPush/{nombreUsuario}/{id}",
                defaults: new
                {
                    controller = "usuario",
                    action = "actualizarTokenPushNotf"
                }
            );



            //envia un mensaje al usuairo segun la direccion del id app,id usuario
            routes.MapRoute(
                name: "AccesoEnviarMensajePush",
                url: "Usuarios/enviarMensajePush",
                defaults: new
                {
                    controller = "usuario",
                    action = "enviarMensajePushAction"
                }
            );


            //obtener el token del usuario
            routes.MapRoute(
                name: "AccesoObtenerTokenUsuario",
                url: "Usuarios/obtenerTokenUsuario/{idReporte}",
                defaults: new
                {
                    controller = "usuario",
                    action = "obtenerTokenUsuarioAction"
                }
            );


        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
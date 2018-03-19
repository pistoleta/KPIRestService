using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceInterface;
using System.Runtime.Remoting.Messaging;
using KPIRestService.Entidades;
using MySql.Data;
using MySql.Data.MySqlClient;
using Base.BD;

namespace KPIRestService.InterfazServicio
{
    public class RegistroService : RestServiceBase<RegistroDTO>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override object OnGet(RegistroDTO request)
        {
            
          /*  Servidor.cola.Enqueue(request);
            Console.WriteLine("OnGet - tamCola: " + Servidor.cola.Count);
            while (Servidor.ocupado) { }
            Servidor.ocupado = true;
               
                request = Servidor.cola.Dequeue();
                Console.WriteLine("OnGet libre - tamCola: " + Servidor.cola.Count);*/

                RegistroDTORespuesta response = new RegistroDTORespuesta();
                //response.Valor = AppHost.miServidor.RepiteElNumero(request.NumRegistro);
                response.LRegistros = AppHost.miServidor.LeerRegistros(request.NumRegistro, request.Tamanyo);
                response.Inicio = request.NumRegistro;
                 // Servidor.ocupado = false;
                // Console.WriteLine("FinOnget tamCola:"+Servidor.cola.Count);
                 return response;
            

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override object OnPost(RegistroDTO request)
        {
            RegistroDTORespuesta response = new RegistroDTORespuesta();
      //      AppHost.miServidor.BloquearPedido(idusuariopeticion, request.LineaPedido); //TOMO POSESION
                 
            
            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override object OnPut(RegistroDTO request)
        {
         RegistroDTORespuesta response = new RegistroDTORespuesta();

    
            return response;
        }
            


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override object OnDelete(RegistroDTO request)
        {
            RegistroDTORespuesta response = new RegistroDTORespuesta();


            return response;
        }
    }
}
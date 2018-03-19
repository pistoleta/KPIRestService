using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace KPIRestService.Entidades
{
     
    [RestService("/registro/{NumRegistro}")]
    [RestService("/registro/{NumRegistro}/{Tamanyo}")]
     

    [DataContractAttribute]
    public class RegistroDTO
    {
        
        [DataMemberAttribute]
        public int NumRegistro { get; set; }
        [DataMemberAttribute]
        public int Tamanyo { get; set; }
        
    }

    [DataContractAttribute]
    public class RegistroDTORespuesta : IHasResponseStatus
    {
        [DataMemberAttribute]
        public int Valor { get; set; }
        [DataMemberAttribute]
        public ushort[] LRegistros { get; set; }
        [DataMemberAttribute]
        public int Inicio { get; set; }
        [DataMemberAttribute]
        public  String Formato { get; set; }
        [DataMemberAttribute]
        public ResponseStatus ResponseStatus { get; set; }

        public RegistroDTORespuesta()
        {
            this.LRegistros = new ushort[] {};
            this.ResponseStatus = new ResponseStatus();
            this.ResponseStatus.ErrorCode = "0";
             
        }
    }
}
using System;
using ServiceStack.WebHost.Endpoints;
using Funq;
using ServiceStack.Logging.Support.Logging;
using IBM.Data.DB2;
using ServiceStack.ServiceInterface.Cors;
using Base.BD;
using log4net;

namespace KPIRestService
{
    /// <summary>
    /// Web Services with http://www.servicestack.net/
    /// 
    /// Configuring ServiceStack to run in stand alone mode.
    /// </summary>
    public class AppHost : AppHostHttpListenerBase
    {
        
       
        ServiceStack.Logging.ILog log;
        private string nombreInstancia = "KPIRestService";
        private int puertoInstancia = 2017;
        public static Servidor miServidor = new Servidor();
        //public static int ContadorPeticiones = 0;

        public string Instancia
        {
            get { return nombreInstancia; }
            set { nombreInstancia = value; }
        }

        public int Puerto
        {
            get { return puertoInstancia; }
            set { puertoInstancia = value; }
        }

        public AppHost()
            : base("MonoTouch RemoteInfo", typeof(KPIRestService.InterfazServicio.RegistroService).Assembly)
        {
            //ServiceStack.Logging.LogManager.LogFactory = new ConsoleLogFactory();
            //log = ServiceStack.Logging.LogManager.GetLogger(GetType());
        }

        public override void Configure(Container container)
        {
            try
            {
                //Registers global CORS Headers
                Plugins.Add(new CorsFeature("*", "GET, POST, PUT, DELETE, OPTIONS", "content-type,accept, nonce,username,passworddigest,created", false));

                this.RequestFilters.Add((httpReq, httpRes, requestDto) =>
                {
                    //Handles Request and closes Responses after emitting global HTTP Headers
                    if (httpReq.HttpMethod == "OPTIONS")
                    {

                    }
                });

                //Permit modern browsers (e.g. Firefox) to allow sending of any REST HTTP Method
                this.SetConfig(new EndpointHostConfig
                {
                    GlobalResponseHeaders = {
                        { "Access-Control-Allow-Origin", "*" },
                        { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                        { "Access-Control-Allow-Headers", "content-type,accept, nonce,username,passworddigest,created,idusuario" },
                    },
                });
                
                //Abrimos pool de conexiones para DB2 y MYSQL
            //    BDConn<DB2Connection>.iniciarBDConn("Server=192.168.32.201:50000;Database=DIST;UID=DB2ADMIN;PWD=geinfor;");
            //    BDConn<MySql.Data.MySqlClient.MySqlConnection>.iniciarBDConn("Server=127.0.0.1;Port=3306;Database=trazabilidad;Uid=trazauser;Pwd=trazapass;");
                
                Console.WriteLine("Iniciando aplicacion...");
                miServidor.Iniciar();
                this.Start("http://*:" + puertoInstancia + "/");
                Console.WriteLine("AppHost configurado " + DateTime.Now.ToString() + ", escuchando en " + puertoInstancia);

               // LogManager.GetLogger("Servidor").Error("Prueba");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Detener()
        {
            try
            {
                if (miServidor != null)
                {
                    miServidor.Detener();
                }
                //Detener pool de conexiones.
            //    BDConn<DB2Connection>.detenerBDConn();
            //    BDConn<MySql.Data.MySqlClient.MySqlConnection>.detenerBDConn();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deteniendo." + ex.StackTrace);
            }
        }
    }
}
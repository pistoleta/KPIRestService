using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace KPIRestService
{
    /**
     *  \brief     Program class
     *  \details   Programa principal
     *  \author    Juan Blasco
     *  \version   1.0
     *  \date      23/11/2017
     *  \copyright AGC License
     */
    class Program
    {
        static void Main(string[] args)
        {
            var appHost = new AppHost();

            try
            {
                //Se pasa el nombre de la aplicacion como parametro
                if (args != null && args.Length == 2)
                {
                    appHost.Instancia = args[0];
                    appHost.Puerto = Int32.Parse(args[1]);
                }

                appHost.Init();
                mostrarPantallaServidor();
                Console.ReadLine();
                appHost.Dispose();
                appHost.Detener();
                Console.WriteLine("Servicio detenido!!!!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error al iniciar el servicio " + ex.StackTrace);
            }
        }

        public static void mostrarPantallaServidor()
        {
            Console.WriteLine("***************************************************************");
            Console.WriteLine("**********           SERVICIO KPIRestService           **********");
            Console.WriteLine("***************************************************************");
            Console.WriteLine("Servicio inciado");
            Console.WriteLine("Pulse \"Enter\" para detener:");

        //    Servidor.LeerTodo();
        }
    }
}
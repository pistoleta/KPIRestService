using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using Base.BD;
using MySql.Data.MySqlClient;
using KPIRestService.Entidades;
using Modbus.Device;
using Modbus.Data;
using Modbus.Utility;
using System.IO.Ports;
using System.Threading;
 

namespace KPIRestService
{
    public class Servidor
    {
        static ushort[] registers;
        static ushort[] registers0;
        static ushort[] registers1;
        static ushort[] registers2;
        static ushort[] registers3;
        static ushort[] registers4;
        static ushort[] registershueco = new ushort[1124];

        static Dictionary<int, String> lRegistros;
        public static Queue<RegistroDTO> cola;
        public static bool ocupado;

        public enum ErrorCode
        {
            NoError,
            NoRegEncontrado,
            ErrorBDatos,            
        }

        public void Iniciar()
        {
            for(int i=0; i < registershueco.Length; i++)
            {
                registershueco[i] = 1;
            }
            //cola = new Queue<RegistroDTO>();
            //ocupado = false;
            new Thread(() =>
            {
                LeerTodo();
            }).Start();

            //Inicializar log4net.
            log4net.GlobalContext.Properties["PathLogs"] = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", string.Empty) +  @"\Logs\";
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", string.Empty) + @"\Config\cfgLogger.xml"));
            lRegistros = new Dictionary<int, string>();
        }

        

        public ushort[] LeerRegistrosOLD(int registro, int nregistros)
        {
            Console.WriteLine("LeerRegistros - desde : " + registro + "  "+nregistros+ " registros");
             
            String ttyname = @"/dev/ttyUSB0";
            using (SerialPort port = new SerialPort(ttyname))
            {
                try
                {
                   
                    // configure serial port
                    port.BaudRate = 9600;
                    port.DataBits = 8;
                    port.Parity = Parity.None;
                    port.StopBits = StopBits.One;
                    port.RtsEnable = true;
                    port.Open();
                   
                    // create modbus master
                    IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                    byte slaveId = 41;
                    ushort startAddress = (ushort)registro;
                    ushort numRegisters = (ushort)nregistros;
                    IAsyncResult result;
                    ushort[] registrosPeticion = null;
                    // leer registros	
                    Action action = () =>
                    {
                          registrosPeticion = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
                       
                    };

                    result = action.BeginInvoke(null, null);

                    if (result.AsyncWaitHandle.WaitOne(3000))
                        Console.WriteLine("Method successful.");
                    else
                    {
                        Console.WriteLine("Method timed out.");
                        throw new TimeoutException();
                    }
                    /*   for (int i = 0; i < registrosPeticion.Length; i++)
                       {
                           byte[] byteArray = BitConverter.GetBytes(registrosPeticion[i]);

                           Console.WriteLine("Register {0}={1}-{2}", startAddress +i, byteArray[0], byteArray[1]);
                       }*/
                    ocupado = false;
                    return registrosPeticion;

                }
                catch (Exception ex)
                {
                    ocupado = false;
                    Console.WriteLine("EXCEPTION " + ex.Message);
                    return null;
                }
            }
          
        }

        /*   private static void LlenarTabla()
           {
               for (int i = 0; i < registers.Length-1; i+=2)
               {
                   lRegistros[i] = ObtenerReal(registers[i], registers[i + 1]).ToString();
               }
           }*/

        public  ushort[] LeerRegistros(int registro, int nregistros)
        {
            Console.WriteLine("LeerRegistros - desde : " + registro + "  " + nregistros + " registros");
            try {  
           
                    ushort[] registrosPeticion = null;
                  
                    registrosPeticion = registers.SubArray(registro, nregistros);

                    return registrosPeticion;

                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine("EXCEPTION " + ex.Message);
                    return null;
                }
            }

        
       

        public static void LeerTodo()
        {

            String ttyname = @"/dev/ttyUSB0";

            ushort i = 0;
            while (true)
            {
                using (SerialPort port = new SerialPort(ttyname))
                {
                    try
                    {

                        // configure serial port
                        port.BaudRate = 9600;
                        port.DataBits = 8;
                        port.Parity = Parity.None;
                        port.StopBits = StopBits.One;
                        port.RtsEnable = true;
                        port.Open();

                        // create modbus master
                        IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                        byte slaveId = 41;
                         
                        ushort numRegisters = 100;

                        // leer registros	
                        if (i == 0)
                        {
                            Console.WriteLine("Leyendo desde {0}  {1} registros", i, numRegisters);
                            registers0 = master.ReadHoldingRegisters(slaveId, i, numRegisters);
                           
                        }
                        else if (i == 100)
                        {
                            Console.WriteLine("Leyendo desde {0}  {1} registros", i, numRegisters);
                            registers1 = master.ReadHoldingRegisters(slaveId, i, numRegisters);
                        }
                        else if (i == 200)
                        {
                            Console.WriteLine("Leyendo desde {0}  {1} registros", i, numRegisters);
                            registers2 = master.ReadHoldingRegisters(slaveId, i, numRegisters);
                        }
                        else if(i==300)
                        {
                            Console.WriteLine("Leyendo desde {0}  {1} registros", i, 12);
                            registers3 = master.ReadHoldingRegisters(slaveId, i, 12);
                        }
                        else if (i == 1436)
                        {
                            Console.WriteLine("Leyendo desde {0}  {1} registros", i, 93);
                            registers4 = master.ReadHoldingRegisters(slaveId, i, 93);
                        }
                        if (registers0 != null && registers1 != null && registers2 != null &&registers3!=null && registers4!=null)
                        {
                            registers = registers0.Concat<ushort>(registers1).ToArray<ushort>()
                                .Concat<ushort>(registers2).ToArray<ushort>()
                                .Concat<ushort>(registers3).ToArray<ushort>()
                                .Concat<ushort>(registershueco).ToArray<ushort>()
                                .Concat<ushort>(registers4).ToArray<ushort>();
                           // LlenarTabla();
                        }

                        for (int j = 0; j < registers.Length; j++)
                            Console.WriteLine("Register {0}={1}", j+1 , registers[j]);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("EXCEPTION " + ex.Message);
                       
                    }
                }
                if (i < 300) i += 100;

                // else if (i == 300) i = 300;

                else if (i >= 300 && i < 1000) i = 1436;
                else if (i == 1436) i = 0;

                Console.WriteLine("i = {0}  ", i);

                Thread.Sleep(2500);
                
            }

        }

   

        private static float ObtenerReal(ushort reg1, ushort reg2)
        {
         //   Console.WriteLine("registers0 32 : {0}", registers0[33 - 1].ToString());
         //   Console.WriteLine("registers0 33 : {0}", registers0[34 - 1].ToString());

            byte[] byteArray1 = BitConverter.GetBytes(reg1);
            byte[] byteArray2 = BitConverter.GetBytes(reg2);

         //   Console.WriteLine("bytearray1 : {0} {1}", byteArray1[0], byteArray1[1]);
         //   Console.WriteLine("bytearray2 : {0} {1}", byteArray2[0], byteArray2[1]);

            byte[] byteArray3 = byteArray1.Concat<byte>(byteArray2).ToArray<byte>();
            // Console.WriteLine("bytearray3 : {0}-{1}-{2}-{3}", byteArray3[0], byteArray3[1], byteArray3[2], byteArray3[3]);

            // Console.WriteLine("BitConverter : {0}", BitConverter.ToSingle(byteArray3, 0));

            return BitConverter.ToSingle(byteArray3, 0);
        }

        
        /// <summary>
        /// Simple Modbus serial RTU master write holding registers example.
        /// </summary>
        public static void ModbusSerialRtuMasterWriteRegisters()
        {
            using (SerialPort port = new SerialPort("COM8"))
            {
                // configure serial port
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.RtsEnable = true;
                port.Open();

                // create modbus master
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                byte slaveId = 1;
                ushort startAddress = 4;
                ushort[] registers = new ushort[] { 1, 2, 3 };

                // write three registers
                master.WriteMultipleRegisters(slaveId, startAddress, registers);
            }
        }

        /// <summary>
        /// Simple Modbus serial ASCII master read holding registers example.
        /// </summary>
        public static void ModbusSerialAsciiMasterReadRegisters()
        {
            
                String ttyname = @"/dev/ttyUSB0";
                Console.WriteLine("previo");
                using (SerialPort port = new SerialPort(ttyname))
                {
                try
                {
                    Console.WriteLine("using");
                    // configure serial port
                    port.BaudRate = 9600;
                    port.DataBits = 8;
                    port.Parity = Parity.None;
                    port.StopBits = StopBits.One;
                    port.RtsEnable = true;
                    port.Open();
                    Console.WriteLine("opened");
                    // create modbus master
                    // IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);
                    IModbusSerialMaster master = ModbusSerialMaster.CreateAscii(port);

                    byte slaveId = 41;
                    ushort startAddress = 1;
                    ushort numRegisters = 100;

                    bool exit = false;
                    while (!exit)
                    {
                        // read five registers		
                        ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                        for (int i = 0; i < numRegisters; i++)
                            Console.WriteLine("Register {0}={1}", startAddress + i, registers[i]);

                       if( Console.ReadLine() == "q") exit=true;
                    }
                }catch(Exception ex){
                    Console.WriteLine("EXCEPTION " + ex.Message);
                }
                }
           

            // output: 
            // Register 1=0
            // Register 2=0
            // Register 3=0
            // Register 4=0
            // Register 5=0
        }


        /// <summary>
        /// Simple Modbus serial ASCII slave example.
        /// </summary>
        public static void StartModbusSerialAsciiSlave()
        {
            using (SerialPort slavePort = new SerialPort("COM2"))
            {
                // configure serial port
                slavePort.BaudRate = 9600;
                slavePort.DataBits = 8;
                slavePort.Parity = Parity.None;
                slavePort.StopBits = StopBits.One;
                slavePort.Open();

                byte unitId = 1;

                // create modbus slave
                ModbusSlave slave = ModbusSerialSlave.CreateAscii(unitId, slavePort);
                slave.DataStore = DataStoreFactory.CreateDefaultDataStore();

                slave.Listen();
            }
        }

        /// <summary>
        /// Simple Modbus serial RTU slave example.
        /// </summary>
        public static void StartModbusSerialRtuSlave()
        {
            using (SerialPort slavePort = new SerialPort("COM2"))
            {
                // configure serial port
                slavePort.BaudRate = 9600;
                slavePort.DataBits = 8;
                slavePort.Parity = Parity.None;
                slavePort.StopBits = StopBits.One;
                slavePort.Open();

                byte unitId = 1;

                // create modbus slave
                ModbusSlave slave = ModbusSerialSlave.CreateRtu(unitId, slavePort);
                slave.DataStore = DataStoreFactory.CreateDefaultDataStore();

                slave.Listen();
            }
        }


        /// <summary>
        /// Modbus serial ASCII master and slave example.
        /// </summary>
        public static void ModbusSerialAsciiMasterReadRegistersFromModbusSlave()
        {
            using (SerialPort masterPort = new SerialPort("COM1"))
            using (SerialPort slavePort = new SerialPort("COM2"))
            {
                // configure serial ports
                masterPort.BaudRate = slavePort.BaudRate = 9600;
                masterPort.DataBits = slavePort.DataBits = 8;
                masterPort.Parity = slavePort.Parity = Parity.None;
                masterPort.StopBits = slavePort.StopBits = StopBits.One;
                masterPort.Open();
                slavePort.Open();

                // create modbus slave on seperate thread
                byte slaveId = 1;
                ModbusSlave slave = ModbusSerialSlave.CreateAscii(slaveId, slavePort);
                Thread slaveThread = new Thread(new ThreadStart(slave.Listen));
                slaveThread.Start();

                // create modbus master
                ModbusSerialMaster master = ModbusSerialMaster.CreateAscii(masterPort);

                master.Transport.Retries = 5;
                ushort startAddress = 100;
                ushort numRegisters = 5;

                // read five register values
                ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                for (int i = 0; i < numRegisters; i++)
                    Console.WriteLine("Register {0}={1}", startAddress + i, registers[i]);
            }

            // output
            // Register 100=0
            // Register 101=0
            // Register 102=0
            // Register 103=0
            // Register 104=0
        }

        /// <summary>
        /// Write a 32 bit value.
        /// </summary>
        public static void ReadWrite32BitValue()
        {
            using (SerialPort port = new SerialPort("COM1"))
            {
                // configure serial port
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                // create modbus master
                ModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                byte slaveId = 1;
                ushort startAddress = 1008;
                uint largeValue = UInt16.MaxValue + 5;

                ushort lowOrderValue = BitConverter.ToUInt16(BitConverter.GetBytes(largeValue), 0);
                ushort highOrderValue = BitConverter.ToUInt16(BitConverter.GetBytes(largeValue), 2);

                // write large value in two 16 bit chunks
                master.WriteMultipleRegisters(slaveId, startAddress, new ushort[] { lowOrderValue, highOrderValue });

                // read large value in two 16 bit chunks and perform conversion
                ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, 2);
                uint value = ModbusUtility.GetUInt32(registers[1], registers[0]);
            }
        }

        public int Detener()
        {
            //Cierra el procesador de hilos de conexion
            /*   
            Base.Threads.ThreadPool.getInstance().Stop();
            salirTimerGeneral = true;
            thTimerGeneral.Join(TOUT_TIMER * 2);
            detenerComunicaciones();
            Estado = EstadoInicio.Detenido;
            */
            return 0;
        }

    }


}

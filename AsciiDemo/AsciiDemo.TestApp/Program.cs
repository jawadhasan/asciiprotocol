using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AsciiDemo.TestApp
{
    class Program
    {

        static byte[] STX = new byte[] { 0x02 };
        static byte[] DLE = new byte[] { 0x10 };
        static byte[] ITEM = new byte[] { 0x31 };//1st item
        static byte[] ETX = new byte[] { 0x03 };
        static byte[] SYMBOL = new byte[] { 0x0F, 0x81, 0xA6, 0x0E };//0F 81 A6 0E

        static void Main(string[] args)
        {
            //Test1();

           var hexBytes = HexStringToByteArray("0F81A60E");
           PrintCommand(hexBytes);


            var content = "018900000000000"; //ABC //018900000000000
            Console.WriteLine($"Content: {content}");
            Console.WriteLine();
            
            //var command = BuildCommand(content);
            var command = GetGSCodeWithFunctionCode(content);

            PrintCommand(command);
            

            //for (int i = 1; i <= 10; i++)
            //{
            //    var command = BuildCommand($"ABC {i}");
            //    Thread.Sleep(5000);

            //    Console.WriteLine();
            //    Console.WriteLine($"sending command {i}...");
            //    SendCommand(command);
            //}

            Console.WriteLine($"Press any key to exit.");
            Console.ReadLine();


        }



        public static void Test1()
        {
            //COMMAND STRUCTURE => <STX><DLE>CONTENT<ETX> => <STX><DLE>ABC<ETX>

            var command = new List<byte>();

            // Add STX to the command >> command += "02";
            command.AddRange(new List<byte>() { Convert.ToByte("2", 16) });


            // Add DLE to the command //command += "10";
            command.AddRange(new List<byte>() { Convert.ToByte("10", 16) });

            // Add Content to the Command
            byte[] contents = System.Text.Encoding.ASCII.GetBytes("ABC");
            command.AddRange(contents);

            // Add ETX to the command //command += "03";
            command.AddRange(new List<byte>() { Convert.ToByte("3", 16) });



            var data = command.ToArray();
            Console.WriteLine(data);
            Console.WriteLine(BitConverter.ToString(data).Replace("-", "")); //outputs hex string to display on console

            var decodedData = Encoding.ASCII.GetString(data);
            Console.WriteLine(decodedData);
        }
  

        public static byte[] BuildCommand(string content)
        {
            //COMMAND STRUCTURE => <STX><DLE>CONTENT<ETX> => <STX><DLE>ABC<ETX>
            var command = new List<byte>();

            //Encoding.GetBytes() method converts a string into a bytes array.
            byte[] contents = Encoding.ASCII.GetBytes(content);


            //build structure
            command.AddRange(STX);
            command.AddRange(DLE);
            command.AddRange(contents);
            command.AddRange(ETX);

            return command.ToArray();

        }
        
        public static void PrintBytes(byte[] command)
        {
            Console.WriteLine();
            Console.WriteLine("Bytes");
            foreach (byte b in command)
            {
                Console.Write(b);
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        public static void PrintCommand(byte[] command)
        {
            //PrintBytes(command);

            Console.WriteLine();
            Console.WriteLine("Hex");
            Console.WriteLine(BitConverter.ToString(command).Replace("-", "")); //outputs hex string to display on console
            Console.WriteLine();

            Console.WriteLine("ASCII");
            var decodedData = GetDecodedData(command);
            Console.WriteLine(decodedData);
            Console.WriteLine();
        }


        //Resources to look for more info
        //1.https://www.c-sharpcorner.com/article/c-sharp-string-to-byte-array/



        static string GetDecodedData(byte[] data)
        {
            //Encoding.GetString() method converts an array of bytes into a string
            return Encoding.ASCII.GetString(data);
        }


        static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }


        // 8904125559301 + 0F 81 A6 0E
        static byte[] GetGSCodeWithFunctionCode(string gsCode)
        {
            var command = new List<byte>();
            byte[] contents = Encoding.ASCII.GetBytes(gsCode);
            
            //build structure
            command.AddRange(STX);
            command.AddRange(DLE);
            command.AddRange(ITEM);
            command.AddRange(contents);
            command.AddRange(SYMBOL);
            command.AddRange(ETX);

            return command.ToArray();
        }


        private static void SendCommand(byte[] command)
        {
            using (TcpClient tcpClient = new TcpClient
            {
                ReceiveTimeout = 5000,
                SendTimeout = 5000
            })
            {
                try
                {
                    Int32 port = 502;
                    IPAddress localAddr = IPAddress.Parse("127.0.0.1");


                    tcpClient.Connect(localAddr, port);

                    NetworkStream stream = tcpClient.GetStream();
                    stream.ReadTimeout = 5000;

                    var watch = Stopwatch.StartNew();

                    var data = command.ToArray();
                    stream.Write(data, 0, data.Length);//command sent

                    var decodedCommand = GetDecodedData(command);
                    Console.WriteLine($"command: {decodedCommand} was sent.");

                    //reading response
                    Byte[] response = new Byte[256];
                    int length = 0;
                    try
                    {
                        length = stream.Read(response, 0, response.Length);
                    }
                    catch (Exception ex)
                    {
                        // timeout happend
                        Console.WriteLine(ex);
                    }

                    if (length > 0 && response[0] == 6) // Got ACk
                    {
                        Console.WriteLine("ACK recieved");
                    }
                    else if (length > 0 && response[0] == 21) // Got NAK
                    {
                        Console.WriteLine("NAK recieved");
                    }

                    Thread.Sleep(1000);

                    stream.Close();
                    watch.Stop();
                    //Console.WriteLine(watch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AsciiDemo.Common;

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
            //starting with a delay, so server is up for listening before we send command
            Console.WriteLine("TCP Client Initiating to Send command....");
            Thread.Sleep(1000);

            //Test1();

            //TEST-2: GetBytes of Hex Command
            Console.WriteLine("Test2: HexCommand");
            var hexBytes = Utils.HexStringToByteArray("0F81A60E");
            //PrintBytesOnConsole(hexBytes);
            TCPClientCommandSender.SendCommand(hexBytes);//Send to TCP Server

            Console.WriteLine("*************************************************");
            //Test-3
            Console.WriteLine("Test3: Text Content within Protocol");
            var content = "ABC"; //OR GTIN - 018900000000000 string...
            var command = BuildCommand(content);//Build a command as per protocol
            //PrintBytesOnConsole(command);
            TCPClientCommandSender.SendCommand(command);


            Console.WriteLine("*************************************************");
            Console.WriteLine("Test4: send 10 commands in a loop");
            for (int i = 0; i < 10; i++)
            {
                var data = $"PayloadOrCommand {i}";
                var counterCommand = BuildCommand(data);//Build command as per protocol
                TCPClientCommandSender.SendCommand(counterCommand);
            }

            Console.WriteLine($"Press any key to exit.");
            Console.ReadLine();
        }





        private static byte[] BuildCommand(string content)
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
        public static void PrintBytesOnConsole(byte[] command)
        {
            Console.WriteLine();

            //Print Individual Bytes
            Console.WriteLine("Printing individual Bytes");
            foreach (byte b in command)
            {
                Console.Write(b);
            }

            //Print HexString of Bytes
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Printing Hex String");
            var hexString = Utils.ByteArrayToHexString(command);
            Console.WriteLine(hexString); //outputs hex string to display on console
            Console.WriteLine();

            //Print ASCII-String of Bytes
            Console.WriteLine();
            Console.WriteLine("Printing ASCII");
            var decodedData = Utils.GetAsciiStringFromByteArray(command);
            Console.WriteLine(decodedData);
            Console.WriteLine();
        }

        //An example to build a command
        private static void Test1()
        {
            //COMMAND STRUCTURE => <STX><DLE>CONTENT<ETX> => <STX><DLE>ABC<ETX>

            var command = new List<byte>();

            // Add STX to the command >> command += "02";
            command.AddRange(new List<byte>() { Convert.ToByte("2", 16) });


            // Add DLE to the command //command += "10";
            command.AddRange(new List<byte>() { Convert.ToByte("10", 16) });

            // Add Content to the Command
            byte[] contents = Encoding.ASCII.GetBytes("ABC");
            command.AddRange(contents);

            // Add ETX to the command //command += "03";
            command.AddRange(new List<byte>() { Convert.ToByte("3", 16) });



            var data = command.ToArray();
            Console.WriteLine(data);
            Console.WriteLine(BitConverter.ToString(data).Replace("-", "")); //outputs hex string to display on console

            var decodedData = Encoding.ASCII.GetString(data);
            Console.WriteLine(decodedData);
        }
    }
}

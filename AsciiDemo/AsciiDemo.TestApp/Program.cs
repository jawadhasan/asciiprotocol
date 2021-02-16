using System;
using System.Collections.Generic;
using System.Text;

namespace AsciiDemo.TestApp
{
    class Program
    {
       

        static void Main(string[] args)
        {
            //Test1();
            //Test2();

            var command = BuildCommand("ABC");
            PrintCommand(command);

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
        public static void Test2()
        {
            //COMMAND STRUCTURE => <STX><DLE>CONTENT<ETX> => <STX><DLE>ABC<ETX>
            byte[] STX = new byte[] { 0x02 };
            byte[] DLE = new byte[] { 0x10 };
            byte[] ITEM = new byte[] { 0x31 };//1st item
            byte[] ETX = new byte[] { 0x03 };

            var command = new List<byte>();

            command.AddRange(STX);
            command.AddRange(DLE);

            // Add Content to the Command
            byte[] contents = System.Text.Encoding.ASCII.GetBytes("ABC");
            command.AddRange(contents);

            command.AddRange(ETX);


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

            var STX = new byte[] { 0x02 };
            var DLE = new byte[] { 0x10 };
            var ITEM = new byte[] { 0x31 };//1st item
            var ETX = new byte[] { 0x03 };

            //Encoding.GetBytes() method converts a string into a bytes array.
            byte[] contents = Encoding.ASCII.GetBytes(content);

            
            //build structure
            command.AddRange(STX);
            command.AddRange(DLE);
            command.AddRange(contents);
            command.AddRange(ETX);

            return command.ToArray();

        }

        public static void PrintCommand(byte[] command)
        {
            Console.WriteLine(command);

            foreach (byte b in command)
            {
                Console.WriteLine(b);
            }

            Console.WriteLine("-----------");


            Console.WriteLine(BitConverter.ToString(command).Replace("-", "")); //outputs hex string to display on console

            //Encoding.GetString() method converts an array of bytes into a string
            var decodedData = Encoding.ASCII.GetString(command);
            Console.WriteLine(decodedData);
        }


        //Resources to look for more info
        //1.https://www.c-sharpcorner.com/article/c-sharp-string-to-byte-array/
    }
}

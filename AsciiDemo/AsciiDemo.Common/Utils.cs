using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsciiDemo.Common
{
    public static class Utils
    {
        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string ByteArrayToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static string GetAsciiStringFromByteArray(byte[] data)
        {
            //Encoding.GetString() method converts an array of bytes into a string
            return Encoding.ASCII.GetString(data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AsciiDemo.Common;

namespace AsciiDemo.TestApp
{
    public static class TCPClientCommandSender
    {
        public static void SendCommand(byte[] command)
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
                    
                    stream.Write(command, 0, command.Length);//command sent

                    var decodedCommand = Utils.GetAsciiStringFromByteArray(command);
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
                        //TODO Client processing of the response
                    }
                    else if (length > 0 && response[0] == 21) // Got NAK
                    {
                        Console.WriteLine("NAK recieved");
                        //TODO Client processing of the response
                    }

                    //Thread.Sleep(1000);

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

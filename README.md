# ASCII Protocol Sample

Builds a simple ASCII Protocl COMMAND STRUCTURE => <STX><DLE>CONTENT<ETX> => <STX><DLE>ABC<ETX> and send ASCII protocl commands to TCP-Server

.NET Solution consists of following projects:

- ASCIIDemo.Common
 
Utils.cs (HexStringToByteArray, ByteArrayToHexString, GetAsciiStringFromByteArray)


-ASCIIDemo.TCPListenerAPP - Console App

References ASCIIDemo.Common for utils.
Program.cs is on basic example TCP-Server code at https://github.com/jawadhasan/modbus-tcpserver. 
Server recieve data from a TCP Client and return randomly ACK(06)/NAK(15) responses.


-ASCIIDemo.TestAPP

This is TCP-Client.
Builds a simple COMMAND STRUCTURE => <STX><DLE>CONTENT<ETX> => <STX><DLE>ABC<ETX> and send commands to TCP-Server.

There is also a NodeJS test TCP-client sample (net package + Buffer.from built-in functions).

check details on the article https://hexquote.com/dare-mighty-things-ascii/





﻿namespace Server_UdpClient
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            Server server = new Server("127.0.0.1", 12345, new ConsoleUserInterface());
            server.Run();
        }
    }
}
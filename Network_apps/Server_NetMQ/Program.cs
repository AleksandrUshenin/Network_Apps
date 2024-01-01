using Server_UdpClient;

namespace Server_NetMQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server_NET_MQ server = new Server_NET_MQ("127.0.0.1", 12345, new ConsoleUserInterface());
            server.Run();
        }
    }
}
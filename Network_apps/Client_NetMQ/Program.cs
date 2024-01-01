using Server_UdpClient;

namespace Client_NetMQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client_NET_MQ client = new Client_NET_MQ("127.0.0.1", 12345, new ConsoleUserInterface());
            client.Run();
        }
    }
}
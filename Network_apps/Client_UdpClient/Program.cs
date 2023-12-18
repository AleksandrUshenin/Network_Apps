using Server_UdpClient;

namespace Client_UdpClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 12345, new ConsoleUserInterface());
            client.Run();
        }
    }
}
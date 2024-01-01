using Server_UdpClient;
using Client_UdpClient;

namespace Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 12345, new ConsoleUserInterface());
            Console.WriteLine(client.Test());
        }
        public void Test() 
        {
            Client client = new Client("127.0.0.1", 12345, new ConsoleUserInterface());
            Console.WriteLine(client.Test());
        }
    }
}
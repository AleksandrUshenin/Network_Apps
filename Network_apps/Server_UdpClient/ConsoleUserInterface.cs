using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server_UdpClient.Interfaces;

namespace Server_UdpClient
{
    public class ConsoleUserInterface : IUserInterface
    {
        public string? GetMessage()
        {
            return Console.ReadLine();
        }

        public void Print(string message)
        {
            Console.WriteLine(message);
        }
        public void WaitingUserAction()
        {
            Console.ReadKey();
        }
    }
}

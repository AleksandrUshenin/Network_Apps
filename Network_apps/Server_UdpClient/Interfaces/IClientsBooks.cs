using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient.Interfaces
{
    public interface IClientsBooks
    {
        bool AddUser(string name, IPEndPoint iPEndPoint);
        bool RemoveUser(string name);
        IPEndPoint GetIpUser(string name);
    }
}

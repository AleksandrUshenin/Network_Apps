using Server_UdpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient.Controllers
{
    internal class ClientsBooks : IClientsBooks
    {
        private Dictionary<string, IPEndPoint> _users;

        public ClientsBooks()
        { 
            _users = new Dictionary<string, IPEndPoint>();
        }
        public bool AddUser(string name, IPEndPoint iPEndPoint)
        {
            if (_users.ContainsKey(name))
                return false;
            _users.Add(name, iPEndPoint);
            return true;
        }

        public IPEndPoint? GetIpUser(string name)
        {
            if (_users.ContainsKey(name))
                return _users[name];
            return null;
        }

        public bool RemoveUser(string name)
        {
            return _users.Remove(name);
        }
    }
}

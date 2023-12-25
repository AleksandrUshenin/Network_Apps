using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IPEndPoint? Ip { get; set; }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}

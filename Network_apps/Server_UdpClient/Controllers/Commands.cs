using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient.Controllers
{
    public enum Commands
    {
        Defalt = 1,
        AddUser = 2,
        RemoveUser = 4,
        Response = 8,
        MessageToUser = 16,
        GetUpDate = 32,
        GetUpDateResponse = 64
    }
}

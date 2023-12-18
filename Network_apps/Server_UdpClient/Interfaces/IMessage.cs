using Server_UdpClient.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient.Interfaces
{
    public interface IMessage
    {
        uint Id { get; set; }
        string MessageText { get; set; }
        string SenderIp { get; set; }
        string UserNameFrom { get; set; }
        string UserNameTo { get; set; }
        int SenderPort { get; set; }
        Commands Command { get; set; }
    }
}

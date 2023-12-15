using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient.Interfaces
{
    public interface IMediator
    {
        void SendMessage(Message message);
        Message GetMessage();
    }
}

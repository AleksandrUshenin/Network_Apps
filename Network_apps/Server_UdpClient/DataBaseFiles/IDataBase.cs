using Server_UdpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient.DataBaseFiles
{
    public interface IDataBase
    {
        bool AddUser(User user);
        bool AddUser(string user);
        bool RemoveUser(User user);
        bool RemoveUser(string user);
        bool AddMessage(string user, IMessage message);
        List<Message> GetUpDate(string user);
        List<Message> GetMessages(User user);
        List<Message> GetMessages(string user);
    }
}

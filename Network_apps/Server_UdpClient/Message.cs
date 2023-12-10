using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient
{
    public class Message
    {
        public uint Id { get; set; }
        public string MessageText { get; set; }
        public string SenderIp { get; set; }
        public int SenderPort { get; set; }
        //public DateTime DateTimeMessage { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}  SenderIp: {SenderIp}  SenderPort: {SenderPort}\nMessageText: {MessageText}";
        }
    }
}

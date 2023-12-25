using Server_UdpClient.Controllers;
using Server_UdpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient
{
    public class Message : IMessage
    {
        public int Id { get; set; }
        public string? MessageText { get; set; }
        public string? SenderIp { get; set; }
        public int SenderPort { get; set; }
        public Commands Command { get; set; }
        public string? UserNameFrom { get; set; }
        public string? UserNameTo { get; set; }
        //public DateTime DateTimeMessage { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}  SenderIp: {SenderIp}  SenderPort: {SenderPort}\nMessageText: {MessageText}";
        }
    }
}

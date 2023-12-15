using Server_UdpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient
{
    public class Mediator : IMediator
    {
        private UdpClient _udpClient;
        private IPEndPoint _iPEndPoint;
        public Mediator(IPEndPoint iPEndPoint, UdpClient udpClient)
        {
            _udpClient = udpClient;
            _iPEndPoint = iPEndPoint;
        }
        public Message GetMessage()
        {
            byte[] buffer = _udpClient.Receive(ref _iPEndPoint);
            string JsonMessage = Encoding.UTF8.GetString(buffer);
            Message messageObj = Json_Convertor.Deserialize(JsonMessage);
            return messageObj;
        }

        public void SendMessage(Message message)
        {
            string mes = Json_Convertor.Serialize(message);
            var buf = Encoding.UTF8.GetBytes(mes);
            _udpClient.Send(buf, buf.Length, _iPEndPoint);
        }
        public void SendMessage(Message message, bool mode)
        {
            string mes = Json_Convertor.Serialize(message);
            var buf = Encoding.UTF8.GetBytes(mes);
            if (mode == true)
                _udpClient.Send(buf, buf.Length);
            else
                _udpClient.Send(buf, buf.Length, _iPEndPoint);
        }
    }
}

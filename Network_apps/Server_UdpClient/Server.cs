using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient
{
    internal class Server
    {
        private readonly string _IP;
        private readonly int _Port;
        private uint _id;
        public Server(string ip, int port)
        {
            this._IP = ip;
            this._Port = port;
            _id = 0;
        }
        public void Run()
        {
            try
            {
                RunServer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void RunServer()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(_IP), _Port);
            Console.WriteLine(" Connection....");
            UdpClient client = new UdpClient(ipPoint);
            Console.WriteLine(" Connected");
            Console.WriteLine($" LocalEndPoint: {client.Client.LocalEndPoint}");
            byte[] buffer = null;
            string message;
            do
            {
                buffer = client.Receive(ref ipPoint);
                string JsonMessage = Encoding.UTF8.GetString(buffer);
                Message messageObj = Json_Convertor.Deserialize(JsonMessage);
                message = messageObj.ToString();
                Console.WriteLine($" Get message: {message}\nInfo: Address: {ipPoint.Address}  Port: {ipPoint.Port}");

                string messegSend;
                if (CurretnIdMessage(messageObj.Id))
                    messegSend = "Server get message";
                else
                    messegSend = $"Lose messege! message id : {messageObj.Id}  last id server {_id}";

                new Thread(() =>
                {
                    Message m = new Message() { Id = _id, MessageText = messegSend, SenderIp = _IP, SenderPort = _Port };
                    string mes = Json_Convertor.Serialize(m);
                    var buf = Encoding.UTF8.GetBytes(mes);
                    client.Send(buf, buf.Length, ipPoint);
                }).Start();
            }
            while (buffer.Length > 0);

            client.Close();
        }
        private bool CurretnIdMessage(uint id)
        {
            if (id != _id + 1)
                return false;
            else
            {
                _id = id;
                return true;
            }
        }
    }
}

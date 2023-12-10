using Server_UdpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_UdpClient
{
    internal class Client
    {
        private readonly string _IP;
        private readonly int _Port;

        private string _IPClient;
        private int _PortClient;
        private uint _id;
        public Client(string ip, int port) 
        {
            this._IP = ip;
            this._Port = port;
            _id = 0;
        }
        public void Run()
        {
            try
            {
                RunClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void RunClient()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(_IP), _Port);
            UdpClient client = new UdpClient();

            Console.WriteLine(" Connection....");
            client.Connect(ipPoint);

            string[] dataClient = client.Client.LocalEndPoint.ToString().Split(':');
            _IPClient = dataClient[0];
            int.TryParse(dataClient[1], out _PortClient);

            Console.WriteLine($" LocalEndPoint: {client.Client.LocalEndPoint}");
            Console.WriteLine($" RemoteEndPoint: {client.Client.RemoteEndPoint}");
            Console.WriteLine(" Connectied");
            
            Thread th = null;
            string message = null;

            do
            {
                message = WriteMessage(th);
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                client.Send(buffer, buffer.Length);

                th = new Thread(() =>
                {
                    byte[] buf = client.Receive(ref ipPoint);
                    Message m = Json_Convertor.Deserialize(Encoding.UTF8.GetString(buf));
                    Console.WriteLine(" Get: " + m.ToString());
                });
                th.Start();
            }
            while (!string.IsNullOrWhiteSpace(message));
            client.Close();
        }
        private string WriteMessage(Thread th)
        {
            if (th != null && th.IsAlive)
            {
                Thread.Sleep(500);
                return WriteMessage(th);
            }
            _id++;
            Console.WriteLine(" Write message: ");
            string messageText = Console.ReadLine();
            Message m = new Message { Id = _id, MessageText = messageText, SenderIp = _IPClient, SenderPort = _PortClient };
            return Json_Convertor.Serialize(m);
        }
    }
}
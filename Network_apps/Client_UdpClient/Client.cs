using Server_UdpClient;
using Server_UdpClient.Controllers;
using Server_UdpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_UdpClient
{
    internal class Client : INetwork
    {
        private readonly string _IP;
        private readonly int _Port;

        private string _IPClient;
        private int _PortClient;
        private uint _id;
        private IUserInterface _userInterface;
        public Client(string ip, int port, IUserInterface userInterface) 
        {
            this._IP = ip;
            this._Port = port;
            _id = 0;
            _userInterface = userInterface;
        }
        public void Run()
        {
            try
            {
                RunClient();
            }
            catch (Exception ex)
            {
                _userInterface.Print(ex.Message);
            }
        }
        private void RunClient()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(_IP), _Port);
            UdpClient client = new UdpClient();

            ControllerClient controller = new ControllerClient(_userInterface, ipPoint, client, WriteUsername());
            controller.Run();

            client.Close();
        }
        //private string? WriteMessage(Thread th)
        //{
        //    if (th != null && th.IsAlive)
        //    {
        //        Thread.Sleep(500);
        //        return WriteMessage(th);
        //    }
        //    _id++;
        //    _userInterface.Print(" Write message: ");
        //    string? messageText = _userInterface.GetMessage();
        //    if (string.IsNullOrWhiteSpace(messageText))
        //    {
        //        return WriteMessage(th);
        //    }
        //    if (messageText.ToLower().Equals("exit"))
        //        return null;
        //    Message m = new Message { Id = _id, MessageText = messageText, SenderIp = _IPClient, SenderPort = _PortClient };
        //    return Json_Convertor.Serialize(m);
        //}
        private string? WriteUsername()
        {
            _userInterface.Print(" Write user name: ");
            string? messageText = _userInterface.GetMessage();
            if (string.IsNullOrWhiteSpace(messageText))
            {
                return WriteUsername();
            }
            return messageText;
        }
    }
}
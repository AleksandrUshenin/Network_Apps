using Server_UdpClient;
using Server_UdpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server_UdpClient.Controllers;

namespace Client_UdpClient
{
    internal class ControllerClient : IController
    {
        private IUserInterface _userInterface;
        private UdpClient _udpClient;
        private IPEndPoint _iPEndPoint;
        private Mediator _mediator;
        private readonly string _IP;
        private uint _id;
        private Logs _logs;
        private string _IPClient;
        private int _PortClient;
        private string _usrName;

        public ControllerClient(IUserInterface userInterface, IPEndPoint iPEndPoint, UdpClient udpClient, string usrName)
        {
            _userInterface = userInterface;
            _iPEndPoint = iPEndPoint;
            _udpClient = udpClient;
            _mediator = new Mediator(_iPEndPoint, _udpClient);
            _logs = new Logs();
            _usrName = usrName;
        }

        public void Run()
        {
            _userInterface.Print(" Connection....");
            _udpClient.Connect(_iPEndPoint);

            string[] dataClient = _udpClient?.Client.LocalEndPoint.ToString().Split(':');
            _IPClient = dataClient[0];
            int.TryParse(dataClient[1], out _PortClient);

            _userInterface.Print($" LocalEndPoint: {_udpClient.Client.LocalEndPoint}");
            _userInterface.Print($" RemoteEndPoint: {_udpClient.Client.RemoteEndPoint}");
            _userInterface.Print(" Connectied");

            Thread? th = null;
            string? message = null;
            Message? mesObj = null;
            do
            {
                mesObj = WriteMessage(th);

                _mediator.SendMessage(mesObj, true);

                th = new Thread(() =>
                {
                    Message mesAnswer = _mediator.GetMessage();
                    _userInterface.Print(" Get: " + mesAnswer.ToString());
                });
                th.Start();
            }
            while (mesObj != null);
        }
        private Message? WriteMessage(Thread th)
        {
            if (th != null && th.IsAlive)
            {
                Thread.Sleep(500);
                return WriteMessage(th);
            }
            _id++;
            _userInterface.Print(" Write message: ");
            string? messageText = _userInterface.GetMessage();
            if (string.IsNullOrWhiteSpace(messageText))
            {
                return WriteMessage(th);
            }
            if (messageText.ToLower().Equals("exit"))
                return null;
            return new Message { Id = _id, MessageText = messageText, SenderIp = _IPClient, SenderPort = _PortClient, Command = Commands.AddUser, UserNameFrom = _usrName, UserNameTo = "Server" };
        }
    }
}

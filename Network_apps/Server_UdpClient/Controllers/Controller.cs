using Server_UdpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Server_UdpClient.Controllers
{
    public class Controller : IController
    {
        private IUserInterface _userInterface;
        private UdpClient _udpClient;
        private IPEndPoint _iPEndPoint;
        private IClientsBooks _clientsBooks;
        private Mediator _mediator;
        private readonly string _IP;
        private uint _id;
        private Logs _logs;

        public Controller(IUserInterface userInterface, IPEndPoint iPEndPoint, UdpClient udpClient, IClientsBooks clientsBooks)
        {
            _userInterface = userInterface;
            _iPEndPoint = iPEndPoint;
            _udpClient = udpClient;
            _clientsBooks = clientsBooks;
            _mediator = new Mediator(_iPEndPoint, _udpClient);
            _logs = new Logs();
        }
        public void Run()
        {
            _userInterface.Print(" Connected");
            _userInterface.Print($" LocalEndPoint: {_udpClient.Client.LocalEndPoint}");

            byte[] buffer = null;
            string message;
            do
            {
                var mesObj = _mediator.GetMessage();

                message = mesObj.ToString();
                _userInterface.Print($" Get message: {message}\nInfo: Address: {_iPEndPoint.Address}  Port: {_iPEndPoint.Port}");

                

                new Thread(() =>
                {
                    _logs.SetLog(mesObj);
                }).Start();

                new Thread(() =>
                {
                    string messegSend;
                    if (CurretnIdMessage(mesObj.Id))
                        messegSend = "Server get message";
                    else
                        messegSend = $"Lose messege! message id : {mesObj.Id}  last id server {_id}";

                    Message mesResponse = new Message() { Id = _id, MessageText = messegSend, SenderIp = _IP, SenderPort = _iPEndPoint.Port, Command = Commands.Response, UserNameFrom = "Server", UserNameTo = mesObj.UserNameFrom };
                    _mediator.SendMessage(mesResponse);
                }).Start();

                if (mesObj.Command != Commands.Defalt)
                    new Thread(() => { DoCommand(mesObj); }).Start();
            }
            while (true);//(buffer.Length > 0);
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
        private void DoCommand(Message message)
        {
            switch (message.Command) 
            {
                case Commands.AddUser:
                    _clientsBooks.AddUser(message.UserNameFrom, _iPEndPoint);
                    break;
                case Commands.RemoveUser:
                    _clientsBooks.RemoveUser(message.UserNameFrom);
                    break;
                case Commands.MessageToUser:
                    SendMessageToUser(message);
                    break;
            }
        }
        private void SendMessageToUser(Message message)
        {
            var user = _clientsBooks.GetIpUser(message.UserNameTo);
            if (user != null)
            {
                _mediator.SendMessage(new Message() { Id = ++_id, Command = Commands.Response, MessageText = "User not found!", UserNameFrom = "Server", 
                    UserNameTo = message.UserNameFrom, SenderIp = _iPEndPoint.Address.ToString(), SenderPort = _iPEndPoint.Port});
                return;
            }
            _mediator.SendMessage(message);
        }
    }
}

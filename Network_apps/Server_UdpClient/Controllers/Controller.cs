using Server_UdpClient.DataBaseFiles;
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
        //private IClientsBooks _clientsBooks;
        private IDataBase _dataBase;
        private Mediator _mediator;
        private readonly string _IP;
        private int _id;
        private Logs _logs;
        //private 

        public Controller(IUserInterface userInterface, IPEndPoint iPEndPoint, UdpClient udpClient, IDataBase dataBase)
        {
            _userInterface = userInterface;
            _iPEndPoint = iPEndPoint;
            _udpClient = udpClient;
            //_clientsBooks = clientsBooks;
            _dataBase = dataBase;
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
                Message mesObj = _mediator.GetMessage();

                message = mesObj.ToString();
                _userInterface.Print($" Get message: {message}\nInfo: Address: {_iPEndPoint.Address}  Port: {_iPEndPoint.Port}");

                

                new Thread(() =>
                {
                    _logs.SetLog(mesObj);
                }).Start();

                //new Thread(() =>
                //{
                //    string messegSend;
                //    if (CurretnIdMessage(mesObj.Id))
                //        messegSend = "Server get message";
                //    else
                //        messegSend = $"Lose messege! message id : {mesObj.Id}  last id server {_id}";

                //    Message mesResponse = new Message() { Id = _id, MessageText = messegSend, SenderIp = _IP, SenderPort = _iPEndPoint.Port, Command = Commands.Response, UserNameFrom = "Server", UserNameTo = mesObj.UserNameFrom };
                //    _mediator.SendMessage(mesResponse);
                //}).Start();

                if (mesObj.Command != Commands.Defalt)
                    new Thread(() => { DoCommand(mesObj); }).Start();
            }
            while (true);//(buffer.Length > 0);
        }
        private bool CurretnIdMessage(int id)
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
            bool answer = false;
            switch (message.Command) 
            {
                case Commands.AddUser:
                    answer = _dataBase.AddUser(message.UserNameFrom);
                    if (answer != false)
                        SendMessageToUser(new Message() { Command = Commands.Response, MessageText = "User added", UserNameFrom = "Server" });
                    else
                        SendMessageToUser(new Message() { Command = Commands.Response, MessageText = "User not added", UserNameFrom = "Server" });
                    break;
                case Commands.RemoveUser:
                    //_dataBase.RemoveUser(message.UserNameFrom);
                    SendMessageToUser(new Message() { Command = Commands.Response, MessageText = "Function remove user doesn't work ", UserNameFrom = "Server" });
                    break;
                case Commands.MessageToUser:
                    //SendMessageToUser(message);
                    answer = _dataBase.AddMessage(message.UserNameTo, message);

                    if (answer != false)
                        SendMessageToUser(new Message() { Command = Commands.Response, MessageText = "Message added", UserNameFrom = "Server" });
                    else
                        SendMessageToUser(new Message() { Command = Commands.Response, MessageText = "Message not added", UserNameFrom = "Server" }); ;
                    break;
                case Commands.GetUpDate:
                    var listMes = _dataBase.GetUpDate(message.UserNameFrom);
                    if (listMes == null)
                        SendMessageToUser(new Message() { Command = Commands.Response, MessageText = "No messages", UserNameFrom = "Server" });
                    else
                        SendMessageToUser(listMes);
                    break;
            }
        }
        private void SendMessageToUser(Message message)
        {
            //var user = _clientsBooks.GetIpUser(message.UserNameTo);
            //if (user != null)
            //{
            //    _mediator.SendMessage(new Message() { Id = ++_id, Command = Commands.Response, MessageText = "User not found!", UserNameFrom = "Server", 
            //        UserNameTo = message.UserNameFrom, SenderIp = _iPEndPoint.Address.ToString(), SenderPort = _iPEndPoint.Port});
            //    return;
            //}
            //_mediator.SendMessage(message);

            _mediator.SendMessage(message);
        }
        private void SendMessageToUser(List<Message> message)
        {
            foreach (var item in message)
            {
                SendMessageToUser(item);
            }
        }
    }
}

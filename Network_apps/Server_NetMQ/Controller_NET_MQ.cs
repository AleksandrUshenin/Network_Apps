using Server_UdpClient.Interfaces;
using System.Net.Sockets;
using System.Net;
using Server_UdpClient.DataBaseFiles;
using Server_UdpClient;
using NetMQ.Sockets;
using Server_UdpClient.Controllers;
using NetMQ;

namespace Server_NetMQ
{
    internal class Controller_NET_MQ : IController
    {
        private IUserInterface _userInterface;
        private UdpClient _udpClient;
        private IPEndPoint _iPEndPoint;
        private IDataBase _dataBase;
        //private Mediator _mediator;

        public Controller_NET_MQ(IUserInterface userInterface, IPEndPoint iPEndPoint, UdpClient udpClient, IDataBase dataBase)
        {
            _userInterface = userInterface;
            _iPEndPoint = iPEndPoint;
            _udpClient = udpClient;
            _dataBase = dataBase;
            //_mediator = new Mediator(_iPEndPoint, _udpClient);
        }
        public void Run2() 
        {
            _userInterface.Print(" Start");

            string tcp = $"tcp://{_iPEndPoint.Address}:{_iPEndPoint.Port}";
            using (PullSocket server = new PullSocket()) 
            {
                server.Bind(tcp);
                while (true) 
                {
                    var data = server.ReceiveFrameString();
                    _userInterface.Print(Json_Convertor.Deserialize(data).ToString());

                    new Thread( () => {
                        using (PushSocket push = new PushSocket())
                        {
                            push.Connect(tcp);
                            string mes = Json_Convertor.Serialize(new Message() { Command = Commands.Response, MessageText = "Send message frome server", UserNameFrom = "Server" });
                            push.SendFrame(mes);
                        }
                        //server.SendFrame(Json_Convertor.Serialize(new Server_UdpClient.Message() { Command = Commands.Response, MessageText = "Send message frome server", UserNameFrom = "Server" })); 
                    }).Start();
                }
            }
            //using (RouterSocket server = new RouterSocket()) 
            //{
            //    server.Bind($"tcp://{_iPEndPoint.Address}:{_iPEndPoint.Port}");

            //    var msg = server.ReceiveMultipartMessage();
            //    //_userInterface.Print(Json_Convertor.Deserialize(msg).ToString());


            //}
        }
        public void Run()
        {
            //Run2();
            //return;
            _userInterface.Print(" Connected");
            _userInterface.Print($" LocalEndPoint: {_udpClient.Client.LocalEndPoint}");

            using (ResponseSocket server = new ResponseSocket())
            {
                string data = $"{_iPEndPoint.Address}:{_iPEndPoint.Port}";
                server.Bind($"tcp://{_iPEndPoint.Address}:{_iPEndPoint.Port}");

                string message;
                do
                {
                    Server_UdpClient.Message mesObj = GetMessage(server);
                    if (mesObj == null)
                        continue;
                    message = mesObj.ToString();
                    _userInterface.Print($" Get message: {message}\nInfo: Address: {_iPEndPoint.Address}  Port: {_iPEndPoint.Port}");

                    if (mesObj.Command != Commands.Defalt)
                        DoCommand(mesObj, server);
                    //new Thread(() => { DoCommand(mesObj, server); }).Start();
                }
                while (true);
            }
        }
        private Server_UdpClient.Message GetMessage(ResponseSocket responseSocket)
        {
            //if (!responseSocket.HasIn)
            //{
            //    Thread.Sleep(2000);
            //    Console.WriteLine(" Sleep...");
            //    return null;
            //}
            bool more;
            string message = responseSocket.ReceiveFrameString(out more);
            Server_UdpClient.Message messageObj = Json_Convertor.Deserialize(message);
            return messageObj;
        }
        private void SendMessage(Server_UdpClient.Message message, ResponseSocket responseSocket)
        {
            string mes = Json_Convertor.Serialize(message);
            responseSocket.SendFrame(mes);
        }
        private void DoCommand(Server_UdpClient.Message message, ResponseSocket responseSocket)
        {
            bool answer = false;
            switch (message.Command)
            {
                case Commands.AddUser:
                    answer = _dataBase.AddUser(message.UserNameFrom);
                    if (answer != false)
                        SendMessage(new Server_UdpClient.Message() { Command = Commands.Response, MessageText = "User added", UserNameFrom = "Server" }, responseSocket);
                    else
                        SendMessage(new Server_UdpClient.Message() { Command = Commands.Response, MessageText = "User not added", UserNameFrom = "Server" }, responseSocket);
                    break;
                case Commands.RemoveUser:
                    //_dataBase.RemoveUser(message.UserNameFrom);
                    SendMessage(new Server_UdpClient.Message() { Command = Commands.Response, MessageText = "Function remove user doesn't work ", UserNameFrom = "Server" }, responseSocket);
                    break;
                case Commands.MessageToUser:
                    //SendMessageToUser(message);
                    answer = _dataBase.AddMessage(message.UserNameTo, message);

                    if (answer != false)
                        SendMessage(new Server_UdpClient.Message() { Command = Commands.Response, MessageText = "Message added", UserNameFrom = "Server" }, responseSocket);
                    else
                        SendMessage(new Server_UdpClient.Message() { Command = Commands.Response, MessageText = "Message not added", UserNameFrom = "Server" }, responseSocket);
                    break;
                case Commands.GetUpDate:
                    var listMes = _dataBase.GetUpDate(message.UserNameFrom);
                    if (listMes == null)
                        SendMessage(new Server_UdpClient.Message() { Command = Commands.Response, MessageText = "No messages", UserNameFrom = "Server" }, responseSocket);
                    else
                        SendMessageToUser(listMes, responseSocket);
                    break;
            }
        }
        private void SendMessageToUser(List<Server_UdpClient.Message> message, ResponseSocket responseSocket)
        {
            foreach (var item in message)
            {
                SendMessage(item, responseSocket);
            }
        }
    }
}

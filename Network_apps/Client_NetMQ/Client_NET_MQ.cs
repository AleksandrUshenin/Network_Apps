using NetMQ.Sockets;
using Server_UdpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Server_UdpClient;
using Server_UdpClient.Controllers;
using NetMQ;

namespace Client_NetMQ
{
    internal class Client_NET_MQ : INetwork
    {
        private readonly string _IP;
        private readonly int _Port;
        private IUserInterface _userInterface;

        public Client_NET_MQ(string ip, int port, IUserInterface userInterface)
        {
            this._IP = ip;
            this._Port = port;
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
        private void RunClient3()
        {
            string name = WriteUsername();
            using (RequestSocket client = new RequestSocket())
            {
                _userInterface.Print(" Connection....");
                client.Connect($"tcp://{_IP}:{_Port}");
                _userInterface.Print(" Connectied");


                Message firstMess = new Message { Id = 0, MessageText = "Connect?", SenderIp = "0000", SenderPort = 0, Command = Commands.AddUser, UserNameFrom = "Client", UserNameTo = "Server" };
                client.SendFrame(Json_Convertor.Serialize(firstMess));
                string mes = client.ReceiveFrameString();
                _userInterface.Print(mes);

                while (true)
                {
                    SendingMessages(client);
                    GettingMessages(client);
                }
            }
        }
        private void RunClient2() 
        {
            string name = "Test DealerSocket";
            string tcp = $"tcp://{_IP}:{_Port}";
            using (var reciver = new PullSocket(tcp))
            using (PushSocket client = new PushSocket())
            {
                client.Connect(tcp);
                while (true)
                {
                    _userInterface.Print(" Send ");
                    Thread.Sleep(3000);
                    Message firstMess = new Message { Id = 0, MessageText = "Connect? client", SenderIp = "0000", SenderPort = 0, Command = Commands.AddUser, UserNameFrom = "Client", UserNameTo = "Server" };
                    client.SendFrame(Json_Convertor.Serialize(firstMess));

                    new Thread(() => {
                        _userInterface.Print(" pull ");
                        string rec = reciver.ReceiveFrameString();
                        _userInterface.Print(Json_Convertor.Deserialize(rec).ToString());
                        //using (PullSocket pull = new PullSocket())
                        //{
                        //    pull.Connect(tcp);
                        //    _userInterface.Print(" pull ");
                        //    string getMes = pull.ReceiveFrameString();
                        //    _userInterface.Print(Json_Convertor.Deserialize(getMes).ToString());
                        //}
                    }).Start();
                }
            }
            //using (DealerSocket client = new DealerSocket())
            //{
            //    _userInterface.Print(" Connection....");
            //    client.Connect($"tcp://{_IP}:{_Port}");
            //    _userInterface.Print(" Connectied");
            //    client.SendFrame(Json_Convertor.Serialize(WriteMessage()));

            //    string res = client.ReceiveFrameString();
            //    _userInterface.Print(Json_Convertor.Deserialize(res).ToString());

            //    client.SendFrame(Json_Convertor.Serialize(WriteMessage()));
            //}
        }
        private void RunClient()
        {
            //RunClient2();
            //return;

            string name = WriteUsername();
            using (RequestSocket client = new RequestSocket())
            {
                _userInterface.Print(" Connection....");
                client.Connect($"tcp://{_IP}:{_Port}");
                _userInterface.Print(" Connectied");


                Message firstMess = new Message { Id = 0, MessageText = "Connect?", SenderIp = "0000", SenderPort = 0, Command = Commands.AddUser, UserNameFrom = "Client", UserNameTo = "Server" };
                client.SendFrame(Json_Convertor.Serialize(firstMess));
                string mes = client.ReceiveFrameString();
                _userInterface.Print(mes);

                while (true)
                {
                    SendingMessages(client);
                    GettingMessages(client);
                }

                //Thread sendMess = new Thread(() => { SendingMessages(client); });
                //sendMess.Start();
                //Thread getMess = new Thread(() => { GettingMessages(client); });
                //getMess.Start();

                //sendMess.Join();
                //Task.WaitAll();
                //Console.WriteLine("Waiting");
                //Console.ReadLine();

            }
        }
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
        private void GettingMessages(RequestSocket requestSocket)
        {
            string mes = requestSocket.ReceiveFrameString();
            _userInterface.Print(Json_Convertor.Deserialize(mes).ToString());
            //while (true)
            //{
            //    _userInterface.Print(" Getting Message: ");
            //    if (!requestSocket.HasIn)
            //    {
            //        Thread.Sleep(2000);
            //        _userInterface.Print(" client wainting");
            //        continue;
            //    }
            //    string getMess = requestSocket.ReceiveFrameString();
            //    Message mes = Json_Convertor.Deserialize(getMess);
            //    _userInterface.Print(" Get: " + mes.ToString());
            //    _userInterface.Print(" --------------- ");
            //}
        }
        private void SendingMessages(RequestSocket requestSocket)
        {
            var mes = WriteMessage();
            requestSocket.SendFrame(Json_Convertor.Serialize(mes));
            //while (true)
            //{
            //    WriteMessage();
            //}
        }
        private Message? WriteMessage()
        {
            _userInterface.Print(" Write message: ");
            string? messageText = _userInterface.GetMessage();
            return new Message { Id = 0, MessageText = messageText, SenderIp = "0000", SenderPort = 0, Command = Commands.AddUser, UserNameFrom = "Client", UserNameTo = "Server" };
        }
    }
}

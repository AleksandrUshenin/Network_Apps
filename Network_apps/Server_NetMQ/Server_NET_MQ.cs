using Server_UdpClient.Controllers;
using Server_UdpClient.Interfaces;
using System.Net.Sockets;
using System.Net;
using Server_UdpClient.DataBaseFiles;

namespace Server_NetMQ
{
    internal class Server_NET_MQ : INetwork
    {
        private readonly string _IP;
        private readonly int _Port;
        private IUserInterface _userInterface;

        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;

        public Server_NET_MQ(string ip, int port, IUserInterface userInterface)
        {
            this._IP = ip;
            this._Port = port;
            this._userInterface = userInterface;

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
        }
        public void Run()
        {
            Task? task = null;
            try
            {
                task = Task.Run(() => { RunServer(); }, _token);
                Exit();
            }
            catch (Exception ex)
            {
                _tokenSource.Cancel();
                if (task.IsCanceled)
                {
                    _userInterface.Print(" The task has been canceled token!");
                }
                _userInterface.Print(ex.Message);
            }
        }
        private void Exit()
        {
            _userInterface.WaitingUserAction();
        }
        private void RunServer()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(_IP), _Port);
            _userInterface.Print(" Connection....");
            UdpClient client = new UdpClient(ipPoint);

            //ClientsBooks clientsBooks = new ClientsBooks();
            DataBase dataBase = new DataBase();

            Controller_NET_MQ controller = new Controller_NET_MQ(_userInterface, ipPoint, client, dataBase);
            controller.Run();

            client.Close();
        }
    }
}

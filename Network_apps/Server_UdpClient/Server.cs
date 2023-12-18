using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server_UdpClient.Interfaces;

using Server_UdpClient.Controllers;

namespace Server_UdpClient
{
    internal class Server : INetwork
    {
        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;

        private readonly string _IP;
        private readonly int _Port;
        private uint _id;
        private Logs _logs;
        private IUserInterface _userInterface;
        public Server(string ip, int port, IUserInterface userInterface)
        {
            this._IP = ip;
            this._Port = port;
            _id = 0;
            _logs = new Logs();
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _userInterface = userInterface;
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
            ClientsBooks clientsBooks = new ClientsBooks();

            Controller controller = new Controller(_userInterface, ipPoint, client, clientsBooks);
            controller.Run();

            client.Close();
        }
    }
}

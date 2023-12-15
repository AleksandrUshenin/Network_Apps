using Server_UdpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Server_UdpClient
{
    public class Logs : ILoger
    {
        public void SetLog(Message message)
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), message.SenderIp);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string fileName = Path.Combine(dir, message.SenderPort.ToString() + ".log");

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate | FileMode.Append ))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"{message.ToString()} {DateTime.Now}");
                }
            }
        }
    }
}

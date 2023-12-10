using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server_UdpClient
{
    public class Json_Convertor
    {
        public static string Serialize(Message message)
        {
            string json = JsonSerializer.Serialize(message);
            return json;
        }
        public static Message Deserialize(string message)
        {
            Message mes = (Message)JsonSerializer.Deserialize(message, typeof(Message));
            return mes;
        }
    }
}

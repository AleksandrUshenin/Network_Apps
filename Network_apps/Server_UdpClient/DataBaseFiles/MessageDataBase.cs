using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient.DataBaseFiles
{
    [Table("Messages")]
    internal class MessageDataBase
    {
        [Key, Column("id")]
        public int Id { get; set; }
        [Column("message")]
        public string? MessageContent { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("Receipt_Status")]
        public bool StatusReceipt { get; set; }
        [Column("From_id_user")]
        public int FromIdUser { get; set; }
        //[Column("Status_Message")]
        //public string? TypeMessage { get; set; }
        [ForeignKey("UserId")]
        public virtual UserDataBase User { get; set; }
    }
}

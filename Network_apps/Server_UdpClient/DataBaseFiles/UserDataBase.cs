using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient.DataBaseFiles
{
    [Table("Users")]
    internal class UserDataBase
    {
        private static uint id = 0;
        [Key, Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("age")]
        public int Age { get; set; }
        public virtual ICollection<MessageDataBase> Messages { get; set; }
    }
}

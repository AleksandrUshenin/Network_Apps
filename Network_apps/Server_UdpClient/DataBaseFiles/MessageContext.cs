using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient.DataBaseFiles
{
    internal class MessageContext : DbContext 
    {
        public DbSet<MessageDataBase> Messages { get; set; }
        public DbSet<UserDataBase> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ClientMail.db");
        }
    }
}

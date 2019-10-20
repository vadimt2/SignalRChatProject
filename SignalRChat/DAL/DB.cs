using Common;
using System.Data.Entity;

namespace DAL.SignalRChat
{
    public class DB : DbContext
    {
        public DB() : base("SignalRChatServer")
        {
            
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Chat> Chats { get; set; }


    }
}
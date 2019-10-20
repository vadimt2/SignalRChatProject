using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Chat
    {
        public int Id { get; set; }

        public string Guid { get; set; }

        public string User { get; set; }

        public string Target { get; set; }

        public string UserConnectionID { get; set; }
        public bool IsActive { get; set; }

        public Chat(string user, string target)
        {
            User = user;

            Target = target;
        }

        public Chat()
        {

        }

    }
}

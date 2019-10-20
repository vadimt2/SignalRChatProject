using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Online { get; set; }

        public User()
        {

        }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}

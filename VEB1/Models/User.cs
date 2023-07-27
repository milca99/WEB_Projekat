using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class User
    {
        public string UserName { get; set; }                                                     // (jedinstveno)
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Role { get; set; }

        public User(string userName, string password, string name, string lastName, string email, DateTime birthday, string role)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            Email = email;
            Birthday = birthday;
            Role = role;
            
        }

        public User() { }
    }
}
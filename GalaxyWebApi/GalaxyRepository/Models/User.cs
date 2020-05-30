using System;
using System.Collections.Generic;

namespace GalaxyRepository.Models
{
    public partial class User
    {
        public User()
        {
            Password = new HashSet<Password>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public double Amount { get; set; }

        public virtual ICollection<Password> Password { get; set; }
    }
}

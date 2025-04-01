using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class LoginEntity : BaseEntity
    {
        public string Email { get; set; }  = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int UserId { get; set; }

        public UserEntity User { get; set; } = null!;

    }
}
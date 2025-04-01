using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Provider { get; set; } = string.Empty;// e.g., "Google", "Facebook", "Local"

        public LoginEntity Login { get; set; } = null!;
    }
}
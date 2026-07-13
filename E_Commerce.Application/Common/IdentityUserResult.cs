using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    //Like DTO
    public class IdentityUserResult
    {
        public IdentityUserResult(string id, string displayName, string email, string username)
        {
            Id = id;
            DisplayName = displayName;
            Email = email;
            Username = username;
        }

        public string Id { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
    }
}

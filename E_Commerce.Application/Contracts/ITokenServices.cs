using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface ITokenServices
    {
        string CreateToken(string userId, string email, string userName, IReadOnlyList<string> roles);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface ICashRepository
    {
        Task<string?> GetAsync(string cashKey, CancellationToken ct = default);
        Task SetAsync(string cashKey, string cashValue, TimeSpan? timeToLive, CancellationToken ct = default);
    }
}

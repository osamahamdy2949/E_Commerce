using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface ICashServices
    {
        Task<string?> GetDataAsync(string cashKay, CancellationToken ct = default);

        Task SetDataAsync(string cashKey, object data, TimeSpan? timeToLive, CancellationToken ct = default);
    }
}

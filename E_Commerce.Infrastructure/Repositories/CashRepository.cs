using E_Commerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class CashRepository : ICashRepository
    {
        private readonly IDatabase _database;
        public CashRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetAsync(string cashKey, CancellationToken ct = default)
        {
            var value = await _database.StringGetAsync(cashKey);

            return value.IsNullOrEmpty ? null : value.ToString();
        }

        public async Task SetAsync(string cashKey, string cashValue, TimeSpan? timeToLive, CancellationToken ct = default)
            => await _database.StringSetAsync(cashKey, cashValue, timeToLive ?? TimeSpan.FromDays(2));
    }
}

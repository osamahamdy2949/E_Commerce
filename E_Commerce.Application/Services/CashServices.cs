using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class CashServices : ICashServices
    {
        private readonly ICashRepository _repository;

        public CashServices(ICashRepository repository)
        {
            _repository = repository;
        }
        public Task<string?> GetDataAsync(string cashKay, CancellationToken ct = default)
            => _repository.GetAsync(cashKay, ct);

        public async Task SetDataAsync(string cashKey, object data, TimeSpan? timeToLive, CancellationToken ct = default)
        {
            var jsonValue = JsonSerializer.Serialize(data , new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await _repository.SetAsync(cashKey, jsonValue, timeToLive, ct);
        }
    }
}

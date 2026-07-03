using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IBasketServices
    {
        Task<Result<BasketDto>> GetBasketAsync(string Id, CancellationToken ct = default!);
        Task<Result<BasketDto>> CreateOrUpdateBasket(BasketDto basket, TimeSpan? timeToLive = default!, CancellationToken ct = default!);
        Task<Result<bool>> DeleteBasketAsync(string Id, CancellationToken ct = default!);
    }
}

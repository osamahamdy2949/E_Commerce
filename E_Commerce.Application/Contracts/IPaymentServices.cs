using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentServices
    {
        Task<Result<BasketDto>> CraeteOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default);
    }
}

using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class BasketService : IBasketServices
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Result<BasketDto>> CreateOrUpdateBasket(BasketDto basket, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var basketEntity = _mapper.Map<CustomerBasket>(basket);

            var result = await _repository.CreateOrUpdateBasketAsync(basketEntity, timeToLive, ct);

            return result == null ? Result<BasketDto>.Fail(Error.Failure("Basket not found")) 
                                  : Result<BasketDto>.Ok(basket);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string Id, CancellationToken ct = default)
        {
            var result = await _repository.DeleteBasketAsync(Id, ct);
            return result ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("Basket not found"));
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string Id, CancellationToken ct = default)
        {
            var basket = await _repository.GetBasketAsync(Id, ct);
            return basket == null ? Result<BasketDto>.Fail(Error.Failure("Basket Not Found"))
                                  : _mapper.Map<BasketDto>(basket);
        }
    }
}

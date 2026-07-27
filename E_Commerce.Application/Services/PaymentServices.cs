using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class PaymentServices : IPaymentServices
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IMapper _mapper;
        private readonly PaymentGatewaySettings _Options;

        public PaymentServices(IBasketRepository basketRepository, IUnitOfWork unitOfWork, 
            IPaymentGateway paymentGateway, IOptions<PaymentGatewaySettings> options, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentGateway = paymentGateway;
            _mapper = mapper;
            _Options = options.Value;
        }
        public async Task<Result<BasketDto>> CraeteOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {
            #region Basket
            //Get Basket 
            var basket = await _basketRepository.GetBasketAsync(basketId, ct);

            //basket Validation
            if (basket == null)
            {
                return Error.NotFound("Basket Not Found", $"Basket With Id {basketId} Not Found");
            }

            if (basket.Items.Count == 0)
            {
                return Error.Validation("Basket Is Empty", "Can't Create Order With Empty Basket");
            }
            #endregion

            #region Delivery Method
            //Get Delivery Method
            if (!basket.DeliveryMethodId.HasValue)
                return Error.Validation("Delivery Method Id Is Required");

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value, ct);

            if (deliveryMethod == null)
                return Error.NotFound("Delivery Method Not Found");

            basket.ShippingPrice = deliveryMethod.Cost;
            #endregion

            #region Product Price
            //Get Products
            var productsIds = basket.Items.Select(p => p.Id).ToHashSet();
            var products = (await _unitOfWork.GetRepository<Product, int>()
                .GetAllAsync(new ProductWithIdSpecifications(productsIds), ct)).ToDictionary(p => p.Id);

            //Price Validation
            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Error.NotFound("Product Not Found");

                item.Price = product.Price;
            }
            #endregion

            // Total Amount
            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);
            var amount = (long)((subTotal + deliveryMethod.Cost) * 100m);

            //Payment
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                //Create
                var currency = _Options.DefaultCurrency;
                var result = await _paymentGateway.CreatePaymentIntentAsync(amount, currency, ct);

                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            else
            {
                //Update
                await _paymentGateway.UpdatePaymentIntentAsync(amount, basket.PaymentIntentId, ct);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket, ct: ct);

            return _mapper.Map<BasketDto>(basket);
        }
    }
}
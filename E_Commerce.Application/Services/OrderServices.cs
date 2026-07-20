using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class OrderServices : IOrderServices
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderServices(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId, ct);

            if (basket == null)
            {
                return Error.NotFound("Basket Not Found", $"Basket With Id {orderDto.BasketId} Is Not Found");
            }

            if(basket.Items.Count == 0)
            {
                return Error.Validation("Basket is Empty", $"Can't Create Order With Basket Id {orderDto.BasketId}");
            }

            var orderItems = new List<OrderItem>(basket.Items.Count);
            var productsIds = basket.Items.Select(p => p.Id).ToHashSet();
            var products = (await _unitOfWork.GetRepository<Product, int>()
                .GetAllAsync(new ProductWithIdSpecifications(productsIds), ct)).ToDictionary(p => p.Id);
            foreach(var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                {
                    return Error.NotFound("product Not Found", $"product With Id {item.Id} Is Not Found");
                }

                orderItems.Add(new OrderItem()
                {
                    Price = product.Price,
                    Quntity = item.Quantity,
                    Product = new ProductItemOrdered()
                    {
                        PictureUrl = product.PictureUrl,
                        ProductName = product.Name,
                        ProductId = product.Id
                    }
                });
            }

            var orderAddress = _mapper.Map<OrderAddress>(orderDto.ShippingAddress);

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if (deliveryMethod == null)
            {
                return Error.NotFound("DeliveryMethod Not Found", $"DeliveryMethod With Id {orderDto.DeliveryMethodId} Is Not Found");
            }

            var subTotal = orderItems.Sum(p => p.Quntity * p.Price);

            var newOrder = new Order(email, orderAddress, orderItems, deliveryMethod, subTotal);

            _unitOfWork.GetRepository<Order, Guid>().Add(newOrder);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            if(result == 0)
            {
                return Error.Failure("Saving Order Failed", "Can't Create This Order");
            }
            else
            {
                await _basketRepository.DeleteBasketAsync(orderDto.BasketId, ct);
                return _mapper.Map<OrderToReturnDto>(newOrder);
            }
        }
    }
}

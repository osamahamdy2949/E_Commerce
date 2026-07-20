using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto, CancellationToken ct)
        {
            return ToActionResult(await _orderServices.CreateOrderAsync(orderDto, GetEmailFromToken(), ct));
        }
    }
}

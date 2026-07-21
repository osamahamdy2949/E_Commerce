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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllUserOrders(CancellationToken ct)
        {
            return ToActionResult(await _orderServices.GetAllOrdersByEmailAsync(GetEmailFromToken(), ct));
        }
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetUserOrderById(Guid id, CancellationToken ct)
        {
            return ToActionResult(await _orderServices.GetOrderByIdAndEmailAsync(id, GetEmailFromToken(), ct));
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethods(CancellationToken ct)
        {
            return ToActionResult(await _orderServices.GetAllDeliveryMethodAsync(ct));
        }
    }
}

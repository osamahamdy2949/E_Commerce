using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class PaymentController : ApiBaseController
    {
        private readonly IPaymentServices _paymentServices;

        public PaymentController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        [Authorize,HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateorUpdatePaymentIntent(string basketId, CancellationToken ct)
            => ToActionResult(await _paymentServices.CraeteOrUpdatePaymentIntentAsync(basketId, ct));
    }
}

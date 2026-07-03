using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketServices _basketServices;

        public BasketsController(IBasketServices basketServices)
        {
            _basketServices = basketServices;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BasketDto>> GetBasketAsync(string id, CancellationToken ct = default)
        {
            var result = await _basketServices.GetBasketAsync(id, ct);
           
            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateBasketAsync([FromBody] BasketDto basketCreateDto, CancellationToken ct = default)
        {
            var result = await _basketServices.CreateOrUpdateBasket(basketCreateDto, ct:ct);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct)
        {
            var result = await _basketServices.DeleteBasketAsync(id, ct);

            return ToActionResult(result);
        }
        
    }
}

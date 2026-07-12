using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.API.Attributes
{
    public class RedisCashAttribute : ActionFilterAttribute
    {
        private readonly int _durationInSeconds;

        public RedisCashAttribute(int durationInSeconds = 60)
        {
            _durationInSeconds = durationInSeconds;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cashServices = context.HttpContext.RequestServices.GetRequiredService<ICashServices>();
            
            var cashKey = CreateCashKey(context.HttpContext.Request);

            var data = await cashServices.GetDataAsync(cashKey);

            if (!string.IsNullOrEmpty(data))
            {
                context.Result = new ContentResult()
                {
                    Content = data,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var excutedContext = await next.Invoke();

            if (excutedContext.Result is OkObjectResult { Value : not null} ok)
            {
                await cashServices.SetDataAsync(cashKey, ok.Value, TimeSpan.FromSeconds(_durationInSeconds));
            }
        }

        private static string CreateCashKey(HttpRequest request)
        {
            var Key = new StringBuilder();

            Key.Append(request.Path);

            if (request.Query.Any())
            {
                Key.Append('?');
                foreach (var (k, v) in request.Query.OrderBy(x=> x.Key))
                {
                    Key.Append(k).Append('=').Append(v).Append('&');
                }
            }

            return Key.ToString();
        }
    }
}

using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Payment
{
    internal class StripPaymentGateway : IPaymentGateway
    {
        private readonly PaymentGatewaySettings _options;
        private readonly PaymentIntentService _paymentIntentService = new();

        public StripPaymentGateway(IOptions<PaymentGatewaySettings> options)
        {
            _options = options.Value;
        }
        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var options = new PaymentIntentCreateOptions()
            {
                Amount = (long)amount,
                Currency = currency,
                PaymentMethodTypes = ["card"]
            };

            var intent = await _paymentIntentService.CreateAsync(options, cancellationToken: ct);

            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }

        public async Task<PaymentIntentResult> UpdatePaymentIntentAsync(decimal amount, string PaymentIntentId, CancellationToken ct = default)
        {
            var options = new PaymentIntentUpdateOptions()
            {
                Amount = (long)amount
            };

            var intent = await _paymentIntentService.UpdateAsync(PaymentIntentId, options);

            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }
    }
}

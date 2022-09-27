using System.Threading.Tasks;
using Autobarn.PricingEngine;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingServer.Services {
    public class PricerService : Pricer.PricerBase {
        private readonly ILogger<PricerService> logger;
        public PricerService(ILogger<PricerService> logger) {
            this.logger = logger;
        }

        public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
            logger.LogInformation(($"Calculating a price for {request.Manufacturer} {request.Model} ({request.Color}, {request.Year})..."));
            return Task.FromResult(new PriceReply {
                CurrencyCode = "EUR",
                Price = 12345
            });
        }
    }
}

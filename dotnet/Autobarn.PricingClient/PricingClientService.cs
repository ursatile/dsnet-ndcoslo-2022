using System;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.PricingEngine;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingClient {
    public class PricingClientService : IHostedService {
        private readonly ILogger<PricingClientService> logger;
        private readonly IBus bus;
        private readonly Pricer.PricerClient grpcClient;

        public PricingClientService(ILogger<PricingClientService> logger, IBus bus, Pricer.PricerClient grpcClient) {
            this.logger = logger;
            this.bus = bus;
            this.grpcClient = grpcClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            logger.LogInformation($"Starting PricingClientService...");
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>($"Autobarn.PricingClient", HandleNewVehicleMessage);
        }

        private async Task HandleNewVehicleMessage(NewVehicleMessage m) {
            logger.LogInformation("Received NewVehicleMessage");
            logger.LogInformation(m.ToString());
            var pr = new PriceRequest {
                Year = m.Year,
                Model = m.ModelName,
                Color = m.Color,
                Manufacturer = m.ManufacturerName
            };
            var priceReply = await grpcClient.GetPriceAsync(pr);
            logger.LogInformation($"Got a price: {priceReply.Price} {priceReply.CurrencyCode}");
            var nvpm = m.WithPrice(priceReply.Price, priceReply.CurrencyCode);
            await bus.PubSub.PublishAsync(nvpm);
            logger.LogInformation($"Published a NewVehiclePriceMessage: {nvpm}");
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            logger.LogInformation($"Stopping PricingClientService...");
            return Task.CompletedTask;
        }
    }
}

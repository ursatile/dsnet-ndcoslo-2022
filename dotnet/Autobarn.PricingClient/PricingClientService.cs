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
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>($"Autobarn.AuditLog_{Environment.MachineName}", HandleNewVehicleMessage);
        }

        private void HandleNewVehicleMessage(NewVehicleMessage m) {
            logger.LogInformation("Received NewVehicleMessage");
            logger.LogInformation(m.ToString());
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            logger.LogInformation($"Stopping PricingClientService...");
            return Task.CompletedTask;
        }
    }
}

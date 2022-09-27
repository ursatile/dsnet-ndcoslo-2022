using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Autobarn.Notifier {
    public class NotifierService : IHostedService {
        private readonly ILogger<NotifierService> logger;
        private readonly IBus bus;
        private readonly HubConnection hub;

        public NotifierService(ILogger<NotifierService> logger, IBus bus, HubConnection hub) {
            this.logger = logger;
            this.bus = bus;
            this.hub = hub;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            logger.LogInformation($"Starting NotifierService...");
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>($"Autobarn.Notifier", HandleNewVehiclePriceMessage);
            await hub.StartAsync();
        }

        private const string user = "autobarn.notifier";

        private async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage m) {
            logger.LogInformation("Received NewVehiclePriceMessage");
            var json = JsonConvert.SerializeObject(m);
            await hub.SendAsync("AnythingWeLikeInHere", user, json);
            logger.LogInformation(m.ToString());
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            logger.LogInformation($"Stopping NotifierService...");
            return Task.CompletedTask;
        }
    }
}

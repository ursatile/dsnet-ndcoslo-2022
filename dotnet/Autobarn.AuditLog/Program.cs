using System;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Autobarn.AuditLog {
    class Program {
        static void Main(string[] args) {

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) => {
                    var amqp = hostContext.Configuration.GetConnectionString("RabbitMQ");
                    var bus = RabbitHutch.CreateBus(amqp);
                    services.AddSingleton(bus);
                    services.AddHostedService<AuditLogService>();
                })
                .Build();
            host.Run();
        }
    }

    public class AuditLogService : IHostedService {
        private readonly ILogger<AuditLogService> logger;
        private readonly IBus bus;

        public AuditLogService(ILogger<AuditLogService> logger, IBus bus) {
            this.logger = logger;
            this.bus = bus;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            logger.LogInformation($"Starting AuditLogService...");
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>($"Autobarn.AuditLog_{Environment.MachineName}", HandleNewVehicleMessage);
        }

        private void HandleNewVehicleMessage(NewVehicleMessage m) {
            logger.LogInformation("Received NewVehicleMessage");
            logger.LogInformation(m.ToString());
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            logger.LogInformation($"Stopping AuditLogService...");
            return Task.CompletedTask;
        }
    }
}

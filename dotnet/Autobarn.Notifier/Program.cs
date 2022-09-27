using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autobarn.Notifier {
    class Program {
        static void Main(string[] args) {

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) => {
                    var hub = new HubConnectionBuilder()
                        .WithUrl(hostContext.Configuration["SignalRHubUrl"])
                        .Build();
                    services.AddSingleton(hub);
                    var amqp = hostContext.Configuration.GetConnectionString("RabbitMQ");
                    var bus = RabbitHutch.CreateBus(amqp);
                    services.AddSingleton(bus);
                    services.AddHostedService<NotifierService>();
                })
                .Build();
            host.Run();
        }
    }
}

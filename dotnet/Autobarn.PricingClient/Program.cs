using System;
using System.Threading.Tasks;
using Autobarn.PricingEngine;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autobarn.PricingClient {
    class Program {
        static async Task Main(string[] args) {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) => {
                    var url = hostContext.Configuration["PricingServerUrl"];
                    var channel = GrpcChannel.ForAddress(url);
                    var grpc = new Pricer.PricerClient(channel);
                    services.AddSingleton(grpc);
                    var amqp = hostContext.Configuration.GetConnectionString("RabbitMQ");
                    var bus = RabbitHutch.CreateBus(amqp);
                    services.AddSingleton(bus);
                    services.AddHostedService<PricingClientService>();
                })
                .Build();
            var runningHost = host.RunAsync();
            if (args.Length > 0 && args[0] == "--console") {
                var random = new Random();
                Console.WriteLine("Running in interactive console mode. Press a key to do a price calculation...");
                while (true) {
                    var grpcClient = host.Services.GetService<Pricer.PricerClient>();
                    var priceReply = await grpcClient.GetPriceAsync(new PriceRequest {
                        Manufacturer = "Test",
                        Model = "Test",
                        Color = "Blue",
                        Year = random.Next(1950, 2022)
                    });
                    Console.WriteLine($"Price: {priceReply.CurrencyCode} {priceReply.Price}");
                    Console.ReadKey(true);
                }
            }
            runningHost.Wait();
        }
    }
}


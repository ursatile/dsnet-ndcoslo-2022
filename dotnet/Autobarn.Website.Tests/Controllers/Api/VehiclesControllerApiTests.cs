using System.Net.Http;
using System.Text;
using Autobarn.Messages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Autobarn.Website.Tests.Controllers.Api
{
    public class VehiclesControllerApiTests :
        IClassFixture<AutobarnTestWebApplicationFactory<Startup>> {
        private readonly AutobarnTestWebApplicationFactory<Startup> factory;
        private readonly HttpClient client;

        public VehiclesControllerApiTests(AutobarnTestWebApplicationFactory<Startup> factory) {
            this.factory = factory;
        }

        [Fact]
        public async void GET_VEhicles_Gets_Vehicles() {
            var client = factory.CreateClient();
            var result = await client.GetAsync("/api/vehicles");
            result.IsSuccessStatusCode.ShouldBe(true);
            var json = await result.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            var finalHref = ((string) data._links.final.href);
            finalHref.ShouldNotBeNull();
            var result2 = await client.GetAsync(finalHref);
        }
    }


    public class ModelsControllerApiTests :
        IClassFixture<AutobarnTestWebApplicationFactory<Startup>> {
        private readonly AutobarnTestWebApplicationFactory<Startup> factory;
        private readonly HttpClient client;

        public ModelsControllerApiTests(AutobarnTestWebApplicationFactory<Startup> factory) {
            this.factory = factory;
        }

        [Fact]
        public async void GET_Models_Returns_Models() {
            var client = factory.CreateClient();
            var result = await client.GetAsync("/api/models");
            result.IsSuccessStatusCode.ShouldBe(true);
        }

        [Fact]
        public async void POST_Publishes_To_Bus() {
        
            var vehicle = new {
                registration = "TEST0001",
                year = 1985,
                color = "Blue"
            };
            var body = new StringContent(JsonConvert.SerializeObject(vehicle), Encoding.UTF8, "application/json");
            var result = await client.PostAsync("/api/models/volkswagen-beetle", body);
            result.IsSuccessStatusCode.ShouldBe(true);
            var pubSub = (FakePubSub) factory.FakeBus.PubSub;
            pubSub.Messages.Count.ShouldBe(1);
            var message = (NewVehicleMessage) (pubSub.Messages[0]);
            message.Registration.ShouldBe("TEST0001");
        }
    }
}

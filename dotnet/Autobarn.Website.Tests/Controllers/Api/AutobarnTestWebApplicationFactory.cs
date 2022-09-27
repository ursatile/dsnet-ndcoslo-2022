using System.Collections.Generic;
using Autobarn.Data;
using Autobarn.Data.Entities;
using EasyNetQ;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Autobarn.Website.Tests.Controllers.Api
{
    public class AutobarnTestWebApplicationFactory<T> : WebApplicationFactory<T> where T : class {
        public IBus FakeBus { get; set; } = new FakeBus();
        
        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            builder.ConfigureServices(services => {
                var models = new List<Model> {
                    new Model() {Name = "Test"}
                };
                var db = new FakeAutobarnDatabase(models);
                services.RemoveAll(typeof(IAutobarnDatabase));
                services.AddSingleton<IAutobarnDatabase>(db);

                services.RemoveAll(typeof(IBus));
                services.AddSingleton(FakeBus);
            });
        }
    }
}

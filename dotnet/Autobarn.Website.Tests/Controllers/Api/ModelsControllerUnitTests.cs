using System.Collections.Generic;
using System.Linq;
using Autobarn.Data.Entities;
using Autobarn.Website.Controllers.api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Autobarn.Website.Tests.Controllers.Api {
    
    public class ModelsControllerUnitTests {
        [Fact]
        public void GET_Returns_Models() {
            var models = new List<Model> {
                new Model() {Name = "Test"}
            };
            var db = new FakeAutobarnDatabase(models);
            var bus = new FakeBus();
            var mc = new ModelsController(db, bus);
            var result = mc.Get() as OkObjectResult;
            result.Value.ShouldBeAssignableTo<IEnumerable<Model>>();
            var resultModels = result.Value as IEnumerable<Model>;
            resultModels.First().Name.ShouldBe("Test");
        }
    }
}

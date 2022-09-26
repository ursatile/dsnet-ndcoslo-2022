using System;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Autobarn.Messages;
using Autobarn.Website.Models;
using EasyNetQ;

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase {
        private readonly IAutobarnDatabase db;
        private readonly IBus bus;

        public ModelsController(IAutobarnDatabase db, IBus bus) {
            this.db = db;
            this.bus = bus;
        }

        [HttpGet]
        public IEnumerable<Model> Get() {
            return db.ListModels();
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound();
            var resource = vehicleModel.ToResource();
            return Ok(resource);
        }

        [HttpPost("{id}")]
        public IActionResult Post(string id, [FromBody] VehicleDto dto) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == null) return NotFound($"There is no vehicle model matching '{id}'");
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel
            };
            db.CreateVehicle(vehicle);
            PublishVehicleMessage(vehicle);
            return Created($"/api/vehicles/{vehicle.Registration}", vehicle.ToResource());
        }

        private void PublishVehicleMessage(Vehicle vehicle) {
            var message = new NewVehicleMessage {
                Registration = vehicle.Registration,
                Color = vehicle.Color,
                Year = vehicle.Year,
                ManufacturerName = vehicle?.VehicleModel?.Manufacturer?.Name ?? "unknown",
                ModelName = vehicle?.VehicleModel?.Name ?? "unknown",
                ListedAt = DateTimeOffset.UtcNow
            };
            bus.PubSub.Publish(message);
        }
    }
}

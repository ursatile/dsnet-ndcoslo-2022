using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.Extensions.Logging;
using ILogger = Castle.Core.Logging.ILogger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
    [Route("api")]
    [ApiController]
    public class DefaultController : ControllerBase {
        [HttpGet]
        public IActionResult Get() {
            return Ok(new {
                message = "Welcome to the Autobarn API",
                _links = new {
                    vehicles = new {
                        href = "/api/vehicles"
                    },
                    models = new {
                        href = "/api/models"
                    }
                }
            });
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase {
        private readonly IAutobarnDatabase db;
        private readonly ILogger<VehiclesController> logger;

        public VehiclesController(IAutobarnDatabase db, ILogger<VehiclesController> logger) {
            this.db = db;
            this.logger = logger;
        }

        const int PAGE_SIZE = 10;
        // GET: api/vehicles
        [HttpGet]
        public IActionResult Get(int index = 0) {
            logger.LogDebug($"GET index={index}");
            var vehicles = db.ListVehicles().Skip(index).Take(PAGE_SIZE)
                .Select(vehicle => vehicle.ToResource());
            var total = db.CountVehicles();
            var _links = Hypermedia.Paginate("/api/vehicles", index, PAGE_SIZE, total);
            return Ok(new {
                _links,
                index,
                count = PAGE_SIZE,
                total,
                vehicles,
            });
        }

        // GET api/vehicles/ABC123
        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            var vehicle = db.FindVehicle(id);
            if (vehicle == default) return NotFound();
            var result = vehicle.ToResource();
            return Ok(result);
        }

        // PUT api/vehicles/ABC123
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] VehicleDto dto) {
            var vehicleModel = db.FindModel(dto.ModelCode);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                ModelCode = vehicleModel.Code
            };
            db.UpdateVehicle(vehicle);
            return Ok(dto);
        }

        // DELETE api/vehicles/ABC123
        [HttpDelete("{id}")]
        public IActionResult Delete(string id) {
            var vehicle = db.FindVehicle(id);
            if (vehicle == default) return NotFound();
            db.DeleteVehicle(vehicle);
            return NoContent();
        }
    }
}

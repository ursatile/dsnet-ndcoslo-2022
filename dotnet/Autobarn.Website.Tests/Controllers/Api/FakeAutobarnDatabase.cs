using System.Collections.Generic;
using Autobarn.Data;
using Autobarn.Data.Entities;

namespace Autobarn.Website.Tests.Controllers.Api
{
    public class FakeAutobarnDatabase : IAutobarnDatabase {
        private readonly IEnumerable<Model> models;

        public FakeAutobarnDatabase(IEnumerable<Model> models) {
            this.models = models;
        }

        public int CountVehicles() => 1;

        public IEnumerable<Vehicle> ListVehicles() {
            return new List<Vehicle> {
                new Vehicle {Registration = "XUNIT001", Color = "Blue", Year = 1985}
            };
        }

        public IEnumerable<Manufacturer> ListManufacturers() {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Model> ListModels() => models;

        public Vehicle FindVehicle(string registration) => default;

        public Model FindModel(string code) {
            return new Model() {
                Code = code
            };
        }

        public Manufacturer FindManufacturer(string code) {
            throw new System.NotImplementedException();
        }

        public void CreateVehicle(Vehicle vehicle) { }

        public void UpdateVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }

        public void DeleteVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }
    }
}

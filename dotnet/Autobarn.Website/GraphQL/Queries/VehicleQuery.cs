using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Queries {

    //public class YearFilter {
    //    public int Year { get; set; }
    //    public string Op { get; set; }

    //    public bool Matches(Vehicle v) {
    //        return Op switch {
    //            ">" => v.Year > Year,
    //            "<" => v.Year < Year,
    //            "=" => v.Year == Year,
    //            _ => true
    //        };
    //    }
    //}

    //public sealed class YearFilterGraphType : IInputObjectGraphType {
    //    public YearFilterGraphType() {
    //        Field(f => f.Year);
    //        Field(f => f.Op);
    //    }
    //}

    public sealed class VehicleQuery : ObjectGraphType {

        private readonly IAutobarnDatabase db;

        public VehicleQuery(IAutobarnDatabase db) {
            this.db = db;

            Field<ListGraphType<VehicleGraphType>>("vehicles")
                .Description("Return all vehicles")
                .Resolve(GetAllVehicles);

            Field<VehicleGraphType>("vehicle")
                .Description("Get a single vehicle")
                .Arguments(MakeNonNullStringArgument("registration", "The registration of the vehicle you want"))
                .Resolve(GetVehicle);


            Field<ListGraphType<VehicleGraphType>>("vehiclesByColor")
                .Description("Get all vehicles matching a certain colour")
                .Arguments(MakeNonNullStringArgument("color", "What color cars do you want?"))
                .Resolve(GetVehiclesByColor);

        }

        private object GetVehiclesByColor(IResolveFieldContext<object> context) {
            var color = context.GetArgument<string>("color");
            if (color.Equals("blurple", StringComparison.InvariantCultureIgnoreCase)) {
                context.Errors.Add(new ExecutionError("You are not allowed to ask for blurple cars."));
            }
            return db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        }


        private QueryArgument MakeNonNullStringArgument(string name, string description) {
            return new QueryArgument<NonNullGraphType<StringGraphType>> {
                Name = name, Description = description
            };
        }

        private Vehicle GetVehicle(IResolveFieldContext<object> context) {
            var reg = context.GetArgument<string>("registration");
            return db.FindVehicle(reg);
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg) {
            return db.ListVehicles();
        }


    }
}

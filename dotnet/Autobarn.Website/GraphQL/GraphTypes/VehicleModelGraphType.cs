using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public sealed class VehicleModelGraphType : ObjectGraphType<Model> {
        public VehicleModelGraphType() {
            Name = "model";
            Field(m => m.Code);
            Field(m => m.Name);
            Field(m => m.Manufacturer, type: typeof(ManufacturerGraphType), nullable: false)
                .Description("The company that manufacturers this model of car, eg. BMW, Tesla");
        }
    }
}

using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes
{
    public sealed class ManufacturerGraphType : ObjectGraphType<Manufacturer> {
        public ManufacturerGraphType() {
            Name = "manufacturer";
            Field(m => m.Code);
            Field(m => m.Name);
        }
    }
}

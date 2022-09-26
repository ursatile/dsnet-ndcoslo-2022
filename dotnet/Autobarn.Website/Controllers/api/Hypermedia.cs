using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Autobarn.Data.Entities;

public static class Hypermedia {
    public static dynamic ToResource(this Vehicle vehicle) {
        var result = vehicle.ToDynamic();
        result._links = new {
            self = new {
                href = $"/api/vehicles/{vehicle.Registration}",
            },
            model = new {
                href = $"/api/models/{vehicle.ModelCode}"
            }
        };
        return result;
    }
    private static dynamic ToDynamic(this object value) {
        IDictionary<string, object> expando = new ExpandoObject();
        var properties = TypeDescriptor.GetProperties(value.GetType());
        foreach (PropertyDescriptor property in properties) {
            if (Ignore(property)) continue;
            expando.Add(property.Name, property.GetValue(value));
        }
        return expando;
    }

    private static bool Ignore(PropertyDescriptor property) {
        return property.Attributes.OfType<Newtonsoft.Json.JsonIgnoreAttribute>().Any();
    }

    public static dynamic Paginate(string baseUrl, int index, int count, int total) {
        dynamic _links = new ExpandoObject();
        _links.self = new {
            href = $"{baseUrl}?index={index}"
        };
        _links.first = new {
            href = $"{baseUrl}?index=0"
        };
        _links.final = new {
            href = $"{baseUrl}?index={total - count}"
        };
        if (index > 0) {
            _links.previous = new {
                href = $"{baseUrl}?index={index - count}"
            };
        }
        if (index + count < total) {
            _links.next = new {
                href = $"{baseUrl}?index={index + count}"
            };
        }
        return _links;
    }
}

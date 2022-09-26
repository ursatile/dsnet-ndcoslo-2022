using System.Dynamic;

public static class Hypermedia {
    public static dynamic Paginate(string baseUrl, int index, int count, int total) {
        dynamic _links = new ExpandoObject();
        _links.self = new {
            href = $"{baseUrl}?index={index}"
        };
        _links.first = new {
            href = $"{baseUrl}?index=0"
        };
        _links.final = new {
            href = $"{baseUrl}?index={total-count}"
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

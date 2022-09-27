using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services;

public class GreeterService : Greeter.GreeterBase {
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger) {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        var name = request.Name;
        var message = request.Language switch {
            "en-GB" => $"Good morning, {name}!",
            "no-NB" => $"God morgen, {name}",
            "en-AU" => $"G'day, {name}",
            _ => $"Hello {name}"
        };

        return Task.FromResult(new HelloReply {
            Message = message
        });
    }
}

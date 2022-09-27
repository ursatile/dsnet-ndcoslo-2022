using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services;

public class GreeterService : Greeter.GreeterBase {
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger) {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        var message = request.Language switch {
            "en-GB" => $"Good morning, {request.Name}!",
            "no-NB" => $"God morgen, {request.Name}",
            "en-AU" => $"G'day, {request.Name}",
            _ => $"Hello {request.Name}"
        };

        return Task.FromResult(new HelloReply {
            Message = message
        });
    }
}

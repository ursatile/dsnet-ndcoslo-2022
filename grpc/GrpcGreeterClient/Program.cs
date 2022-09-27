using GrpcGreeter;
using Grpc.Net.Client;
using System.Threading.Tasks;

using var channel = GrpcChannel.ForAddress("https://localhost:7043");
var client = new Greeter.GreeterClient(channel);
while(true) {
    Console.WriteLine("Press any key to send a gRPC request:");
    var language = Console.ReadKey(true).KeyChar switch {
        '1' => "en-GB",
        '2' => "no-NB",
        '3' => "en-AU",
        _ => String.Empty
    };
    var reply = client.SayHello(new HelloRequest {
        Language = language,
        FirstName = "NDC",
        LastName = "Oslo"
    });
    Console.WriteLine(reply.Message);
}



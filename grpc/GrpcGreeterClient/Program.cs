using GrpcGreeter;
using Grpc.Net.Client;
using System.Threading.Tasks;

using var channel = GrpcChannel.ForAddress("https://localhost:7043");
var client = new Greeter.GreeterClient(channel);
while(true) {
    Console.WriteLine("Press any key to send a gRPC request:");
    Console.ReadKey(true);
    var reply = client.SayHello(new HelloRequest {
        Name = "NDC Oslo"
    });
    Console.WriteLine(reply.Message);
}



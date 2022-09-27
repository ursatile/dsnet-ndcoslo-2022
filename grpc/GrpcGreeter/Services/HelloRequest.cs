namespace GrpcGreeter;

public sealed partial class HelloRequest {
    public string Name => $"{FirstName} {LastName}";
}

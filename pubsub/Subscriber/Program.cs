using EasyNetQ;
using Messages;

const string AMQP = "amqps://aqbgvtbw:DLZt9nFoITcQrb-ddTm6duOOs7EHnwF3@large-yellow-chameleon.rmq4.cloudamqp.com/aqbgvtbw";
var bus = RabbitHutch.CreateBus(AMQP);
bus.PubSub.Subscribe<Greeting>("dylan_beattie", Handle);
Console.WriteLine("Listening for messages...");
Console.ReadKey(false);

void Handle(Greeting greeting) {
    if (greeting.Number % 5 == 0) {
        throw new Exception("Something weird happened!");
    }
    Console.WriteLine(greeting);
}

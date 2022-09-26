using EasyNetQ;
using Messages;

const string AMQP = "amqps://aqbgvtbw:DLZt9nFoITcQrb-ddTm6duOOs7EHnwF3@large-yellow-chameleon.rmq4.cloudamqp.com/aqbgvtbw";
Console.WriteLine("Press any key to publish a message...");
var bus = RabbitHutch.CreateBus(AMQP);

int number = 0;
while(true) {
    var greeting = new Greeting {
        Number = number++
    };
    bus.PubSub.Publish(greeting);
    Console.WriteLine($"Published: {greeting}");
    Console.ReadKey(true);
}

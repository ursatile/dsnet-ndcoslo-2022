using EasyNetQ;
using Messages;

const string AMQP = "amqps://aqbgvtbw:DLZt9nFoITcQrb-ddTm6duOOs7EHnwF3@large-yellow-chameleon.rmq4.cloudamqp.com/aqbgvtbw";
var bus = RabbitHutch.CreateBus(AMQP);
bus.PubSub.Subscribe<Greeting>("SUBSCRIPTION_ID", Console.WriteLine);
Console.WriteLine("Listening for messages...");
Console.ReadKey(false);

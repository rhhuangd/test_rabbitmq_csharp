using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory{HostName = "localhost"};
var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "pubsub",
    type: ExchangeType.Fanout);

var message = "Hello I want to broadcast this message";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(
    exchange:"pubsub", 
    routingKey: "", 
    mandatory: false,
    basicProperties: null,
    body: body);

Console.WriteLine($"Send message: {message}");
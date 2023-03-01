using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// establish connection
var factory = new ConnectionFactory(){HostName="localhost"};
var connection = factory.CreateConnection();

// create channel
var channel = connection.CreateModel();

// set up a consumer for Client requests
channel.QueueDeclare("request-queue", exclusive: false, autoDelete: false);
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (ch, e) => 
{
    Console.WriteLine($"Request received: {e.BasicProperties.CorrelationId}");
    var reply = $"This is a reply to request: {e.BasicProperties.CorrelationId}";
    var body = Encoding.UTF8.GetBytes(reply);
    channel.BasicPublish("", e.BasicProperties.ReplyTo, null, body);
};
channel.BasicConsume("request-queue", true, consumer);
Console.WriteLine("Server - Start Consuming");

Console.ReadKey();
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// establish connection
var factory = new ConnectionFactory(){HostName="localhost"};
var connection = factory.CreateConnection();

// create channel
var channel = connection.CreateModel();

// set up a consumer for Server replies
var reply_queue = channel.QueueDeclare();
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (channel, e) => 
{
    var body = e.Body.ToArray();
    var reply = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Reply received: {reply}");
};
channel.BasicConsume(reply_queue.QueueName, true, consumer);
Console.WriteLine("Client - Start Consuming");

// set up a producer for Client requests
channel.QueueDeclare(queue: "request-queue", exclusive: false, autoDelete: false);
var request = "This is a client request";
var body = Encoding.UTF8.GetBytes(request);
var properties = channel.CreateBasicProperties();
properties.ReplyTo = reply_queue.QueueName;
properties.CorrelationId = Guid.NewGuid().ToString();
channel.BasicPublish("", "request-queue", properties, body);
Console.WriteLine($"Client - Send request: {properties.CorrelationId}");

Console.ReadKey();
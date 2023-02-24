using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory{HostName="localhost"};
var connection = factory.CreateConnection();

var channel = connection.CreateModel();

// channel.ExchangeDeclare("sharpRouting", ExchangeType.Direct);
channel.ExchangeDeclare("sharpTopicExchange", ExchangeType.Topic);
var queue = channel.QueueDeclare();
// channel.QueueBind(queue.QueueName, "sharpRouting", "paymentsonly");
channel.QueueBind(queue.QueueName, "sharpTopicExchange", "#.payments");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (channel, e) => 
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Payments Consumer - Received message: {message}");
};

channel.BasicConsume(queue.QueueName, true, consumer);

Console.WriteLine("Start consuming");
Console.ReadKey();
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory{HostName="localhost"};
var connection = factory.CreateConnection();

var channel = connection.CreateModel();

// channel.ExchangeDeclare("sharpRouting", ExchangeType.Direct);
channel.ExchangeDeclare("sharpTopicExchange", ExchangeType.Topic);

// var message = "This is a broadcast message (analyticsonly)";
// var message = "This is a broadcast message (paymentsonly)";
var message = "This is a broadcast message (topic exchange)";
var body = Encoding.UTF8.GetBytes(message);

// channel.BasicPublish("sharpRouting", "analyticsonly", false, null, body);
// channel.BasicPublish("sharpRouting", "paymentsonly", false, null, body);
channel.BasicPublish("sharpTopicExchange", "user.taiwan.payments", false, null, body);
Console.WriteLine($"Send message: {message}");

channel.BasicPublish("sharpTopicExchange", "business.taiwan.payments", false, null, body);
Console.WriteLine($"Send message: {message}");

channel.BasicPublish("sharpTopicExchange", "user.america.payments", false, null, body);
Console.WriteLine($"Send message: {message}");

connection.Close();
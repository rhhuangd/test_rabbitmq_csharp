using System;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;

// 1. create the connection
var factory = new ConnectionFactory{HostName = "localhost"};
using var connection = factory.CreateConnection();

// 2. create channels
using var channel = connection.CreateModel();

// 3. declare queues
channel.QueueDeclare(
    queue: "queue1", 
    durable: false, 
    exclusive: false, 
    autoDelete: false, 
    arguments: null);

var random = new Random();
var messageID = 1;

// 4. create messages
while(true)
{
    var processing_time = random.Next(1,4);
    string msg = $"Sending message: {messageID}";
    var message = Encoding.UTF8.GetBytes(msg);

// 5. publish the messages
    channel.BasicPublish(
        exchange: "", 
        routingKey: "queue1", 
        basicProperties: null, 
        body: message);

    Console.WriteLine($"Send message: {msg}");
    Task.Delay(TimeSpan.FromSeconds(processing_time)).Wait();
    messageID++;
}

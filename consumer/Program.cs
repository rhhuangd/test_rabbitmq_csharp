using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
    arguments: null
);

// 4. create consumers 
var consumer = new EventingBasicConsumer(channel);

// 5. binding callback functions with message-received events of consumers  
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"message received: {message}");
};

//6. binding consumers to channels 
channel.BasicConsume(
    queue: "queue1", 
    autoAck: true, 
    consumer: consumer);

Console.ReadKey();
using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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
    arguments: null
);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, false);

// 4. create consumers 
var consumer = new EventingBasicConsumer(channel);

var random = new Random();

// 5. binding callback functions with message-received events of consumers  
consumer.Received += (model, ea) =>
{
    var processing_time = random.Next(1, 6);
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received: {message} will take {processing_time} to process");
    Task.Delay(TimeSpan.FromSeconds(processing_time)).Wait();
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};

//6. binding consumers to channels 
channel.BasicConsume(
    queue: "queue1", 
    autoAck: false, 
    consumer: consumer);

Console.WriteLine("Consuming");

Console.ReadKey();
using System;
using System.Text;
using RabbitMQ.Client;

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

// 4. create messages
string msg = "c# first message";
var message = Encoding.UTF8.GetBytes(msg);

// 5. publish the messages
channel.BasicPublish(
    exchange: "", 
    routingKey: "queue1", 
    basicProperties: null, 
    body: message);


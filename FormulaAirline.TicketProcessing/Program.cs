﻿// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Welcome to the ticketing service");

var factory = new ConnectionFactory()
    {
        HostName = "localhost",
        UserName = "user",
        Password = "mypass",
        VirtualHost = "/", 
    };

var conn = factory.CreateConnection();
using var chanel = conn.CreateModel();
chanel.QueueDeclare("bookings", durable: true, exclusive: false);

var consumer = new EventingBasicConsumer(chanel);

consumer.Received += (model, eventArgs) => 
{
    // getting my byte[]
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"New ticket processing is initiated for - {message}");
};

chanel.BasicConsume("bookings", true, consumer);
Console.ReadKey();

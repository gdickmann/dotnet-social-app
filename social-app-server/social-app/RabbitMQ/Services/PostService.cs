﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace social_app.RabbitMQ.Services
{
    public class PostService : BackgroundService
    {

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "posts",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("[x] Received {0}", message);
            };

            channel.BasicConsume(queue: "posts",
                                    autoAck: true,
                                    consumer: consumer);

            return Task.CompletedTask;
        }
    }
}

using CommandsService.EventProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandsService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private IEventProcessor _eventProcessor;
        private IConfiguration _configuration;
        private string _queuename;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _eventProcessor = eventProcessor;
            _configuration = configuration;

            var factory = new ConnectionFactory();
            factory.HostName = _configuration["RabbitMQHost"];
            factory.Port = int.Parse(_configuration["RabbitMQPort"]);
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _queuename = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queuename,
                    exchange: "trigger",
                    routingKey: "");
                Console.WriteLine($"--> Listening on the Message Bus....");

                _connection.ConnectionShutdown += _connection_ConnectionShutdown;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to Rabbit MQ :{ex.Message}");
            }
        }

        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Rabbit MQ Connction Shutdown");
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received !");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(notificationMessage);
            };
            _channel.BasicConsume(queue: _queuename, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}

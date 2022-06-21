using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory();
            factory.HostName = _configuration["RabbitMQHost"];
            factory.Port = int.Parse(_configuration["RabbitMQPort"]);
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += _connection_ConnectionShutdown;
                Console.WriteLine($"--> Connected to Rabbit MQ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to Rabbit MQ :{ex.Message}");
            }
        }

        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"Rabbit Connection Shutdown");
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"--> Message sent {message}");
        }

        private void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        public void PublishNewPlatform(PlatformPublishDto platformPublish)
        {
            try
            {
                var message = JsonSerializer.Serialize(platformPublish);

                if (_connection.IsOpen)
                {
                    Console.WriteLine("--> RabbitMQ connection open, sending message....");
                    SendMessage(message);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to send message {e.Message}");
            }
        }
    }
}

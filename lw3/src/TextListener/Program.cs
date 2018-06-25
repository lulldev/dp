using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;

namespace TextListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                Console.WriteLine(" [*] Waiting for messages...");

                while(true)
                {             
                    channel.QueueDeclare(queue: "backend-api",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        string id = Encoding.UTF8.GetString(body);
                        string value = GetValueById(id);
                        Console.WriteLine(" [x] Received: {0} {1}", id, value);
                    };
                    channel.BasicConsume(queue: "backend-api",
                                        autoAck: true,
                                        consumer: consumer);                        
                }
            }
        }

        private static string GetValueById(string id) 
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            return redis.GetDatabase().StringGet(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Diagnostics;
using StackExchange.Redis;
using RabbitMQ.Client;
using System.Text;

namespace Backend.Controllers
{
    [Route("api/[controller]")] 
    public class ValuesController : Controller
    {
        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            return redis.GetDatabase().StringGet(id);
        }

        // POST api/values
        [HttpPost]
        public string Post([FromForm]string value)
        {
            var id = Guid.NewGuid().ToString();
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
                redis.GetDatabase().StringSet(id, value);
                PublishMessage(id);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return id;
        }

        private void PublishMessage(string id)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "backend-api",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var body = Encoding.UTF8.GetBytes(id);

                channel.BasicPublish(exchange: "",
                                    routingKey: "backend-api",
                                    basicProperties: null,
                                    body: body);

                Console.WriteLine(" [x] Sent: {0}", id);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace SendingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs",
                    type: "topic");

                foreach (var arg in args)
                {
                    var parts = arg.Split(new[] { ':' }, StringSplitOptions.None);
                    if (parts.Length < 2) continue;
                    var serverity = parts[0];
                    var message = parts[1];
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "topic_logs",
                        routingKey: serverity,
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", serverity, message);
                }
            }

            Console.ReadKey();
        }
    }
}

using System;
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
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                foreach (var arg in args)
                {
                    var message = GetMessage(new[] {arg});
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "logs",
                        routingKey: "",
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0)
                ? string.Join(" ", args)
                : "info: Hello World!");
        }
    }
}

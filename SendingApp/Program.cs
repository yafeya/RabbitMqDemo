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
            var severityDic = new Dictionary<int, string>();
            severityDic[0] = "info";
            severityDic[1] = "warning";
            severityDic[2] = "error";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                    type: "direct");

                for (int i = 0; i < args.Length; i++)
                {
                    var severityIndex = i % 3;
                    var severity = severityDic[severityIndex];
                    var message = args[i];
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "direct_logs",
                        routingKey: severity,
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}

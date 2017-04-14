using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Subscriber
{
    class Program
    {
        private const string ExchangeName = "messages";
        private const string HostName = "rabbitmq";

        private const string QueueName = "consumer_queue";

        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = HostName };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: ExchangeName, type: "fanout");

                var queueName = channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false).QueueName;
                channel.QueueBind(queue: queueName,
                                exchange: ExchangeName,
                                routingKey: "");

                Console.WriteLine(" [*] Waiting for files.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);

                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume(queue: queueName, noAck: false, consumer: consumer);

                Console.WriteLine(" Press [CTRL-C] to exit.");
                Console.CancelKeyPress += (sender, e) => _closing.Set();
                _closing.WaitOne();
            }
        }
    }
}

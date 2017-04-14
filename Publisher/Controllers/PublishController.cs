using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Publisher.Controllers
{
    [Route("api/publish")]
    public class PublishController : Controller
    {
        private const string ExchangeName = "messages";
        private const string HostName = "rabbitmq";

        [HttpPost]
        public void Post([FromBody]string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            var factory = new ConnectionFactory() { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(ExchangeName, "fanout");
                channel.BasicPublish(exchange: ExchangeName, routingKey: "", basicProperties: null, body: body);
            }
        }
    }
}

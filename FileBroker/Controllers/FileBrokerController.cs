using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace FileBroker.Controllers
{
    [Route("api/files")]
    public class FileController : Controller
    {
        [HttpPost]
        public void Post([FromBody]string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("files", "fanout");
                    channel.BasicPublish(exchange: "files", routingKey: "", basicProperties: null, body: body);
                }
            }
        }
    }
}

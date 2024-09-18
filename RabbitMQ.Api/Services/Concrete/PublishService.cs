using Newtonsoft.Json;
using RabbitMQ.Api.Services.Abstract;
using RabbitMQ.Client;
using RabbitMQ.Shared.Dto;
using RabbitMQ.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Api.Services.Concrete
{
    public class PublishService : IPublishService
    {
        public async Task PublishQuee(EmailSendRequestDto dto)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri(RabbitMQConnection.Url);

            //Connection active

            using var connection = connectionFactory.CreateConnection();
            using var channnel = connection.CreateModel();

            channnel.QueueDeclare(queue: RabbitMQConst.EmailQueue, exclusive: false, durable: true);

            IBasicProperties properties = channnel.CreateBasicProperties();
            properties.Persistent = true;

            var json = JsonConvert.SerializeObject(dto);

            byte[] message = Encoding.UTF8.GetBytes(json);

            channnel.BasicPublish(exchange: "", routingKey: RabbitMQConst.EmailQueue, body: message, basicProperties: properties);

        }
    }
}

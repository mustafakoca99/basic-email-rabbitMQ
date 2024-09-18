using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Shared.Dto;
using RabbitMQ.Shared.Utils;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Connection
            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri(RabbitMQConnection.Url);

            //Connection active

            using var connection = connectionFactory.CreateConnection();
            using var channnel = connection.CreateModel();

            //Queue oluşurma
            channnel.QueueDeclare(queue: RabbitMQConst.EmailQueue, exclusive: false, durable: true);

            var consumer = new EventingBasicConsumer(channnel);

            channnel.BasicConsume(queue: RabbitMQConst.EmailQueue, autoAck: false, consumer: consumer);
            channnel.BasicQos(0, 1, false);
            consumer.Received += Consumer_Received;

            void Consumer_Received(object? sender, BasicDeliverEventArgs e)
            {
                Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

                SendMail(Encoding.UTF8.GetString(e.Body.Span));

                channnel.BasicAck(e.DeliveryTag, multiple: false);
            }

            Console.Read();

            async void SendMail(string message)
            {
                var messageDto = JsonConvert.DeserializeObject<EmailSendRequestDto>(message);

                var mail = new MailMessage();

                mail.To.Add(messageDto.ToMailAddress);
                mail.CC.Add(messageDto.CcMailAddress);
                mail.Body = messageDto.Body;
                mail.Subject = messageDto.Subject;
                mail.From = new MailAddress(RabbitMQConst.SenderEmailAddress, RabbitMQConst.SenderName, Encoding.UTF8);
                var smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential(RabbitMQConst.SenderEmailAddress, RabbitMQConst.SenderEmailPassword);
                smtp.EnableSsl = true;
                smtp.Host = RabbitMQConst.EmailHost;

                await smtp.SendMailAsync(mail);

            }
        }
    }
}

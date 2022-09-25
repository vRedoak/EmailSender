using EmailSender.Helpers;
using EmailSender.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public class MessageReceiver : DefaultBasicConsumer

    {

        private readonly IModel _channel;

        public MessageReceiver(IModel channel)

        {

            _channel = channel;

        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange,
            string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)

        {
            var message = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine(" [x] Received {0}", message);

            var emailModels = JsonConvert.DeserializeObject<List<EmailModel>>(message);

            foreach (var emailModel in emailModels)
                EmailSendingHelper.SendEmailMessages(emailModel);

            _channel.BasicAck(deliveryTag, false);
        }

    }
}

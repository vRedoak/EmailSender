using EmailSender;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.100.4"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.BasicQos(0, 1, false);

            var messageReceiver = new MessageReceiver(channel);
            channel.BasicConsume(queue: "AllergyTrack.Emails",
                                 autoAck: false,
                                 consumer: messageReceiver);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
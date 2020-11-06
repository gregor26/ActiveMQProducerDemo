using Apache.NMS;
using System;

namespace ActiveMQProducerDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("ActiveMQ Producer Demo");

            string topic = "TextQueue";

            Console.WriteLine($"Adding message to queue topic: {topic}");

            string brokerUri = $"activemq:tcp://192.168.250.198:61616";  // Default port
            NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

            using (IConnection connection = factory.CreateConnection("artemis", "simetraehcapa"))
            {
                var msg = $"a log message {DateTime.UtcNow}";

                connection.Start();

                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue(topic))
                using (IMessageProducer producer = session.CreateProducer(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

                    producer.Send(session.CreateTextMessage(msg));
                    Console.WriteLine($"Sent {msg} messages");
                }
            }

            Console.WriteLine($"");
            Console.WriteLine($"Press any key to finish.");
            Console.ReadKey(true);
        }
    }
}
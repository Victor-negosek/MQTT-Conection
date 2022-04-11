using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Threading.Tasks;

namespace MQTTPublisher
{
    class PubliSubssher
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            IMqttClient client = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                            .WithClientId(Guid.NewGuid().ToString())
                            .WithTcpServer("test.mosquitto.org", 1883)
                            .WithCleanSession()
                            .Build();

            client.UseConnectedHandler(e =>
            {
                Console.WriteLine("Connected to the broker sucessfully");
            });

            client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected from the broker successfully");
            });

            await client.ConnectAsync(options);

            Console.WriteLine("Please press a key to publish the message");

            Console.ReadLine();

            await PublishMessageAsync(client);

            await client.DisconnectAsync();
        }

        private static async Task PublishMessageAsync(IMqttClient client)
        {
            string messagePayload = "Hello!";
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("RishabhSharma")
                .WithPayload(messagePayload)
                .WithAtLeastOnceQoS()
                .Build();
            if (client.IsConnected)
            {
                await client.PublishAsync(message);
            }
        }
    }
}
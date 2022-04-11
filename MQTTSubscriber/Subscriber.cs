using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MQTTSubscriber
{
    class Subscriber
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            IMqttClient client = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                            .WithClientId("ConnectedMachineClient")
                            .WithTcpServer("mqtt.dpart.svc.fortknox.local", 1883)
                            .WithCleanSession()
                            .Build();


            client.UseConnectedHandler(async e =>
            {
                Console.WriteLine("Connected to the broker sucessfully");
                var topicFilter = new TopicFilterBuilder()
                                    .WithTopic("$SYS/broker/uptime")
                                    .Build();
                await client.SubscribeAsync(topicFilter);
            });

            client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected from the broker successfully");
            });

            client.UseApplicationMessageReceivedHandler(e =>
            {
                var message = ($"Received Message - {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine(message);
            });

            await client.ConnectAsync(options);

            Console.ReadLine();

            await client.DisconnectAsync();
        }
    }
}

using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedX509Device
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "mkiothub01.azure-devices.net";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n.");

            // AMQPプロトコルでの接続    
            String cerFilePath = @"C:\temp\azuretest.cer";
            X509Certificate2 x509Certificate = new X509Certificate2(cerFilePath);
            var authMethod = new DeviceAuthenticationWithX509Certificate("MKDevice02", x509Certificate);
            deviceClient = DeviceClient.Create(iotHubUri, authMethod);
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            double avgWindSpeed = 10; // m/s
            Random rand = new Random();

            while (true)
            {
                double currentWindSpeed = avgWindSpeed + rand.NextDouble() * 4 - 2;

                var telemetryDataPoint = new
                {
                    deviceId = "MKDevice02",
                    windSpeed = currentWindSpeed
                };

                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                Task.Delay(1000).Wait();

            }
        }
    }
}

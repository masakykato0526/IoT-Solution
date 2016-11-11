using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedX509Device
{
    class Program
    {
        static DeviceClient deviceClient;

        // 接続するIoT Hub
        static string iotHubUri = "<IoT Hubホスト名>";

        // 送信するデバイスID
        static string deviceId = "<デバイスID>";

        // X.509証明書
        static string cerFilePath = @"<X.509証明書へのパス>";

        static void Main(string[] args)
        {
            // X.509証明書を使用して、HTTPプロトコルで接続するDeviceClientインスタンスを作成
            var x509Certificate = new X509Certificate2(cerFilePath);
            var authMethod = new DeviceAuthenticationWithX509Certificate(deviceId, x509Certificate);

            deviceClient = DeviceClient.Create(iotHubUri, authMethod, TransportType.Http1);

            // メッセージの送信処理
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
                    deviceId = deviceId,
                    windSpeed = currentWindSpeed
                };

                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > 送信メッセージ： {1}", DateTime.Now, messageString);

                Task.Delay(1000).Wait();

            }
        }
    }
}

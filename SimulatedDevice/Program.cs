using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Microsoft.Azure.Devices.Common.Security;

namespace SimulatedDevice
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "mkiothub01.azure-devices.net";
        static string deviceKey = "MHYnxSsE1DeUB0XuDTBDjakE+cOASdMEXKhJ3eNrll4=";

        static string policyName = "iothubowner";
        static string sharedAccessKey = "fGFNhgX6eeJq3BM8epN/sQ8K2vahGlAO1Exo0cUKf+k=";
        static string ttlPeriod = "5";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n.");

            // デバイスIDの対象キーを私用してトークンを生成
            // AMQPプロトコルでの接続
            //deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("MKDevice01", deviceKey));
            // HTTPプロトコルでの接続
            //deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("MKDevice01", deviceKey), TransportType.Http1    );

            // カスタムでShared Access Signatureトークンを生成
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithToken("MKDevice01", CreateSASToken()));
            
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }

        static string CreateSASToken()
        {
            var sasToken = new SharedAccessSignatureBuilder()
            {
                KeyName = policyName,
                Key = sharedAccessKey,
                Target = iotHubUri,
                TimeToLive = TimeSpan.FromDays(Convert.ToDouble(ttlPeriod))
            }.ToSignature();

            Console.WriteLine("SAS: {0}", sasToken);
            return sasToken;
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
                    deviceId = "MKDevice01",
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

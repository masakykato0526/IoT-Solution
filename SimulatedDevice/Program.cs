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

        // 接続するIoT Hub
        static string iotHubUri = "<IoT Hubホスト名";

        // デバイスID
        static string deviceId = "<デバイスID>";
      
        // 共有アクセスポリシーとアクセスキー
        static string policyName = "<共有アクセスポリシー>";
        static string sharedAccessKey = "<アクセスキー>";
        
        // セキュリティトークンの有効期間
        static string ttlDay = "<有効期間>"; //日

        // デバイスキー
        //static string deviceKey = "<デバイスキー>";
    
        static void Main(string[] args)
        {
            // SASトークンを使用して、HTTPプロトコルで接続するDeviceClientインスタンスを作成
            var authMethod = new DeviceAuthenticationWithToken(deviceId, CreateSASToken());

            deviceClient = DeviceClient.Create(iotHubUri, authMethod, TransportType.Http1);

            /*
            // SASトークンを使用して、AMQPプロトコルで接続するDeviceClientインスタンスを作成
            var authMethod = new DeviceAuthenticationWithToken(deviceId, CreateSASToken());
            deviceClient = DeviceClient.Create(iotHubUri, authMethod);

            // SASトークンを使用して、MQTTプロトコルで接続するDeviceClientインスタンスを作成
            var authMethod = new DeviceAuthenticationWithToken(deviceId, CreateSASToken());
            deviceClient = DeviceClient.Create(iotHubUri, authMethod, TransportType.Mqtt);

            // デバイスキーを使用して、AMQPプロトコルで接続するDeviceClientインスタンスを作成
            //deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));
            */

            // メッセージの送信処理
            SendDeviceToCloudMessagesAsync();

            Console.ReadLine();
        }

        static string CreateSASToken()
        {
            // SASトークンを生成
            var sasToken = new SharedAccessSignatureBuilder()
            {
                KeyName = policyName,
                Key = sharedAccessKey,
                Target = iotHubUri + "/devices/" + deviceId,
                TimeToLive = TimeSpan.FromDays(Convert.ToDouble(ttlDay))
            }.ToSignature();

            Console.WriteLine("生成されたSASトークン： {0}", sasToken);
            Console.WriteLine();

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

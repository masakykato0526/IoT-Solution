using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System;

namespace CreateDeviceIdentity
{
    /*
        デバイスIDを指定して、セキュリティトークン形式で認証するデバイスを登録
    */
    class Program
    {
        static RegistryManager registryManager;

        // 作成するデバイスID
        static string deviceId = "<デバイスID>";

        // IoT Hubへの接続文字列
        static string connectString = "<IoT Hub接続文字列>";

        static void Main(string[] args)
        {
            try
            {
                // RegistryManagerを生成
                registryManager = RegistryManager.CreateFromConnectionString(connectString);

                // デバイスの登録処理呼び出し
                AddDeviceAsync().Wait();
            } catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }

            Console.ReadLine();
        }

        private static async Task AddDeviceAsync()
        {
            var device = new Device(deviceId);

            try
            {
                // レジストリへのデバイスの登録
                device = await registryManager.AddDeviceAsync(device);
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            Console.WriteLine("デバイス {0} が生成されました", deviceId);
            Console.WriteLine("  生成されたデバイスキー： {0}", device.Authentication.SymmetricKey.PrimaryKey);
            Console.ReadKey();
        }
    }
}


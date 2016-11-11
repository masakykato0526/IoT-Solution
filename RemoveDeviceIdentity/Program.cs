using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace RemoveDeviceIdentity
{
    /*
        デバイスIDを指定してデバイスを削除
    */
    class Program
    {
        static RegistryManager registryManager;
        // 削除するデバイスID
        static string deviceId = "<デバイスID>";

        // IoT Hubへの接続文字列
        static string connectString = "<IoT Hub接続文字列>";

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectString);
            RemoveDeviceAsync().Wait();

            Console.ReadLine();
        }

        private static async Task RemoveDeviceAsync()
        {
            try
            {
                await registryManager.RemoveDeviceAsync(deviceId);
                Console.WriteLine("デバイス {0} は削除されました", deviceId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }  
        }
    }
}


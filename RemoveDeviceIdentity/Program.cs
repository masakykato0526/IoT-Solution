using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace RemoveDeviceIdentity
{
    class Program
    {
        static RegistryManager registryManager;
        // iothubownerの接続文字列
        static string connectString = "HostName=mkiothub01.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=fGFNhgX6eeJq3BM8epN/sQ8K2vahGlAO1Exo0cUKf+k=";

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectString);
            RemoveDeviceAsync().Wait();

            Console.ReadLine();
        }

        private static async Task RemoveDeviceAsync()
        {
            string deviceId = "MKDevice02";

            try
            {
                await registryManager.RemoveDeviceAsync(deviceId);
                Console.WriteLine("Device {0} is removed", deviceId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }  
        }
    }
}


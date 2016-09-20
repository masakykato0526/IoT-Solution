using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System;

namespace CreateDeviceIdentity
{
    class Program
    {
        static RegistryManager registryManager;
        // iothubownerの接続文字列
        static string connectString = "HostName=mkiothub01.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=fGFNhgX6eeJq3BM8epN/sQ8K2vahGlAO1Exo0cUKf+k=";
        
        static void Main(string[] args)
        {
            try
            {
                registryManager = RegistryManager.CreateFromConnectionString(connectString);
                AddDeviceAsync().Wait();
            } catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }

            Console.ReadLine();
        }

        private static async Task AddDeviceAsync()
        {
            string deviceId = "MKDevice02";
            Device device = new Device(deviceId);

            try
            {
                device = await registryManager.AddDeviceAsync(device);
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}


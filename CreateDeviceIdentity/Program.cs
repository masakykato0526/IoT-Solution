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
        static string connectString = "HostName=mkiothub01.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=fGFNhgX6eeJq3BM8epN/sQ8K2vahGlAO1Exo0cUKf+k=";

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectString);
            AddDeviceAsync().Wait();

            Console.ReadLine();
        }

        private static async Task AddDeviceAsync()
        {
            string deviceId = "MKDevice02";
            Device device;

            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}


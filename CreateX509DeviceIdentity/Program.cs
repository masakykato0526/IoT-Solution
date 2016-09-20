using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateX509DeviceIdentity
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }

            Console.ReadLine();
        }

        private static async Task AddDeviceAsync()
        {
            string deviceId = "MKDevice02";
            Device device = new Device(deviceId)
            {
                /*
                  証明書は以下のPowerShellコマンドにて作成
                   New-SelfSignedCertificate -CertStoreLocation "Cert:\CurrentUser\My" -Subject "Azure Test Cert"
                   // Thumbprint C66C92DEF8A7AD4FA2665B13FD422227C6F078A1の証明書が作成
                   Get-ChildItem Cert:\CurrentUser\My
                   // 証明書一覧の確認
                   Export-Certificate -Cert cert:\CurrentUser\my\C66C92DEF8A7AD4FA2665B13FD422227C6F078A1 -FilePath C:\temp\azuretest.cer
                */
                Authentication = new AuthenticationMechanism()
                {
                    X509Thumbprint = new X509Thumbprint()
                    {
                        //PrimaryThumbprint = "921BC9694ADEB8929D4F7FE4B9A3A6DE58B0790B"
                        PrimaryThumbprint = "C66C92DEF8A7AD4FA2665B13FD422227C6F078A1"
                    }
                }
            };

            try
            {
                device = await registryManager.AddDeviceAsync(device);
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            Console.WriteLine("Generated device");
        }
    }
}
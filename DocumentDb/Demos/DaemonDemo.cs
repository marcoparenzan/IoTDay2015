using DocumentDb.Model;
using DocumentDb.Tools;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentDb.Tools;

namespace DocumentDb.Demos
{
    public static class DaemonDemo
    {
        public static async Task RunAsync(string[] args)
        {
            if (!args.Any())
            {
                await StartDaemonsAsync();
            }
            else
            {
                await RunDaemonAsync(args[0], args[1], args[2], args[3], args[4], args[5]);
            }
        }

        private static async Task StartDaemonsAsync()
        {
            var reader = new StreamReader("sensors.csv");
            while (true)
            {
                var sensorRow = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(sensorRow)) break;
                var sensorInfo = sensorRow.Split(';');
                Process.Start(new ProcessStartInfo
                {
                    Arguments = string.Join(" ", sensorInfo)
                    ,
                    FileName = "DocumentDb.exe"
                });
            }
        }

        private static async Task RunDaemonAsync(string locationName, string locationRegion, string locationLongitude, string locationLatitude, string sensorId, string sensorClass)
        {
            var client = DocumentDbExtension.CreateClient();
            var database = await client.GetDatabaseAsync("IoTEvents");
            var temperatureSamples = await client.GetCollectionAsync(database, "temperatureSamples");

            var location = new Location { Name = locationName, Region = (Region)Enum.Parse(typeof(Region), locationRegion, true), Longitude = double.Parse(locationLongitude), Latitude = double.Parse(locationLatitude) };
            var sensor = new Sensor { Id = Guid.Parse(sensorId), Class = (SensorClass)Enum.Parse(typeof(SensorClass), sensorClass, true) };
            var random = new Random();
            while (true)
            {
                var response = await client.NewTemperatureSampleDocumentAsync(temperatureSamples.SelfLink, location, sensor, random.NextDouble() * 10 + 15);
            }
        }
    }
}

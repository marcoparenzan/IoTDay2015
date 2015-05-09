using DocumentDb.Model;
using DocumentDb.Tools;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Partitioning;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDb.Demos
{
    public static class InsertDemo
    {
        public static async Task RunAsync(string[] args)
        {
            var client = DocumentDbExtension.CreateClient();
            var database = await client.GetDatabaseAsync("IoTEvents");
            var temperatureSamplesInsert = await client.GetCollectionAsync(database, "temperatureSamplesInsert");

            var response = await client.NewTemperatureSampleDocumentAsync(temperatureSamplesInsert.SelfLink, "Pordenone", Region.North, 45, 12, Guid.NewGuid(), SensorClass.A, 24);

            Console.ReadLine();
        }
    }
}

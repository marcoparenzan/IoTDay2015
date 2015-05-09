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
    public static class PartitionDemo
    {
        public static async Task RunAsync(string[] args)
        {
            var client = DocumentDbExtension.CreateClient();
            var database = await client.GetDatabaseAsync("IoTEvents");
            var temperatureSamplesNorth = await client.GetCollectionAsync(database, "temperatureSamplesNorth");
            var temperatureSamplesMiddle = await client.GetCollectionAsync(database, "temperatureSamplesMiddle");
            var temperatureSamplesSouth = await client.GetCollectionAsync(database, "temperatureSamplesSouth");
            RangePartitionResolver<string> regionResolver = new RangePartitionResolver<string>(
                "RegionName",
                new Dictionary<Range<string>, string>() 
                {
                    { new Range<string>("North"), temperatureSamplesNorth.SelfLink },
                    { new Range<string>("Middle"), temperatureSamplesMiddle.SelfLink },
                    { new Range<string>("South"), temperatureSamplesSouth.SelfLink }
                }
            );
            client.PartitionResolvers[database.SelfLink] = regionResolver;

            //var response = await client.NewTemperatureSampleDocumentAsync(database.SelfLink, "Pordenone", Region.North, 45, 12, Guid.NewGuid(), SensorClass.A, 24);
            //response = await client.NewTemperatureSampleDocumentAsync(database.SelfLink, "Roma", Region.Middle, 45, 12, Guid.NewGuid(), SensorClass.B, 25);
            //response = await client.NewTemperatureSampleDocumentAsync(database.SelfLink, "Napoli", Region.South, 45, 12, Guid.NewGuid(), SensorClass.C, 26);

            var query = client.CreateDocumentQuery<TemperatureSample>(database.SelfLink, null, "Middle");
            var sample = query.AsEnumerable().FirstOrDefault();


            Console.ReadLine();
        }
    }
}

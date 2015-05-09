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

namespace DocumentDb.Demos
{
    public static class NormalizeDocumentTriggerDemo
    {
        public static async Task RunAsync(string[] args)
        {
            var client = DocumentDbExtension.CreateClient();
            var database = await client.GetDatabaseAsync("IoTEvents");
            var temperatureSamples = await client.GetCollectionAsync(database, "temperatureSamples");

            var trigger = new Trigger
            {
                Id = "normalize",
                Body = File.ReadAllText(@"Javascript\trigger.Normalize.js"),
                TriggerOperation = TriggerOperation.Create,
                TriggerType = TriggerType.Pre
            };

            await client.TryDeleteTrigger(temperatureSamples.SelfLink, trigger.Id);
            trigger = await client.CreateTriggerAsync(temperatureSamples.SelfLink, trigger);


            var requestOptions = new RequestOptions { PreTriggerInclude = new List<string> { trigger.Id } };
            await client.CreateDocumentAsync(temperatureSamples.SelfLink, new
            {
                sensorId = "53fae456-9afc-4af8-9018-981237430147"
                , degreeValue = 99
                , locationName = "Udine2"
                , locationRegion = 0
                , locationLat = 45
                , locationLong = 12
                , calendarDateTime = "08/05/2015"
                , id = Guid.NewGuid().ToString()
            }, requestOptions);

            var results = client.CreateDocumentQuery<Document>(temperatureSamples.SelfLink, "SELECT * FROM temperatureSamples s WHERE s.Location.Name='Udine2'");
            foreach (var result in results)
            {
                Console.WriteLine("{0}", result);
            }

            Console.ReadLine();
        }
    }
}

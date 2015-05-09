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
    public static class CountStoredProcedureDemo
    {
        public static async Task RunAsync(string[] args)
        {
            var client = DocumentDbExtension.CreateClient();
            var database = await client.GetDatabaseAsync("IoTEvents");
            var temperatureSamples = await client.GetCollectionAsync(database, "temperatureSamples");

            var sp = new StoredProcedure
            { 
                Id = "count"
                ,
                Body = File.ReadAllText(@"Javascript\sp.Count.js")
            };

            await client.TryDeleteStoredProcedure(temperatureSamples.SelfLink, sp.Id);
            sp = await client.CreateStoredProcedureAsync(temperatureSamples.SelfLink, sp);

            var sql = "SELECT * FROM temperatureSamples";
            var continuationToken = string.Empty;
            var count = 0;
            while(true)
            {
                var response = (await client.ExecuteStoredProcedureAsync<dynamic>(sp.SelfLink, sql, continuationToken)).Response;
                count += (int)response.count;
                continuationToken = (string)response.continuationToken;
                if (string.IsNullOrEmpty(continuationToken)) break;
                Console.WriteLine("Partial count: {0}, continuing...", count);
            }

            Console.ReadLine();
        }
    }
}

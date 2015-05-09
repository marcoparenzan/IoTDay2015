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
    public static class CollectionsDemo
    {
        public static async Task RunAsync(string[] args)
        {
            var client = DocumentDbExtension.CreateClient();
            var database = await client.GetDatabaseAsync("IoTEvents");
            var temperatureSamples = await client.GetCollectionAsync(database, "temperatureSamples");

            string continuation = null;
            List<DocumentCollection> collections = new List<DocumentCollection>();

            do
            {
                FeedOptions options = new FeedOptions
                {
                    RequestContinuation = continuation,
                    MaxItemCount = 300                    
                };

                // var response = await client.ReadDocumentCollectionFeedAsync(database.CollectionsLink, options);
                var response = await client.ReadDocumentFeedAsync(temperatureSamples.DocumentsLink, options);
                foreach (DocumentCollection col in response)
                {
                    collections.Add(col);
                }

                continuation = response.ResponseContinuation;
                Console.WriteLine(response.RequestCharge);

            } while (!String.IsNullOrEmpty(continuation));


            Console.ReadLine();
        }
    }
}

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
    public static class QueriesDemo
    {
        public static async Task RunAsync(string[] args)
        {
            var client = DocumentDbExtension.CreateClient();
            var database = await client.GetDatabaseAsync("IoTEvents");
            var temperatureSamples = await client.GetCollectionAsync(database, "temperatureSamples");
            temperatureSamples.IndexingPolicy.IncludedPaths.Add(new IndexingPath { 
                IndexType = IndexType.Hash
                , Path = ""
            });

            var sqlQuerySpec = new SqlQuerySpec{
                QueryText = "SELECT * FROM templeratureSamples s WHERE s.Location.Name = 'Pordenone'"
            };

            var query = client.CreateDocumentQuery<TemperatureSample>(temperatureSamples.DocumentsLink, sqlQuerySpec);
            var items = query.ToList();

            Console.ReadLine();
        }
    }
}

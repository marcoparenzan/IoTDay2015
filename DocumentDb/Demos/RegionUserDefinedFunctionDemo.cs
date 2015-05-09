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
    public static class RegionUserDefinedFunctionDemo
    {
        public static async Task RunAsync(string[] args)
        {
            var client = DocumentDbExtension.CreateClient();
            var database = await client.GetDatabaseAsync("IoTEvents");
            var temperatureSamples = await client.GetCollectionAsync(database, "temperatureSamples");

            var udf = new UserDefinedFunction
            { 
                Id = "region"
                ,
                Body = File.ReadAllText(@"Javascript\udf.Region.js")
            };

            await client.TryDeleteUDF(temperatureSamples.SelfLink, udf.Id);
            var result = await client.CreateUserDefinedFunctionAsync(temperatureSamples.SelfLink, udf);

            var sqlQuerySpec = new SqlQuerySpec{
                QueryText = "SELECT s.Sensor.Id, udf.region(s) as regione FROM s"
            };

            var query = client.CreateDocumentQuery<dynamic>(temperatureSamples.SelfLink, sqlQuerySpec);
            var items = query.ToList();


            Console.ReadLine();
        }
    }
}

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DocumentDb.Tools
{
    public static class DocumentDbExtension
    {
        public static DocumentClient CreateClient()
        {
            var client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["documentDbUri"]), ConfigurationManager.AppSettings["documentDbAuthKey"]);
            return client;
        }

        public static async Task<DocumentCollection> GetCollectionAsync(this DocumentClient client, Database database, string id)
        {
            var documentCollection = client.CreateDocumentCollectionQuery(database.SelfLink).ToList().SingleOrDefault(xx => xx.Id == id);
            if (documentCollection == null)
                documentCollection = await client.CreateDocumentCollectionAsync(database.SelfLink, new DocumentCollection { Id = id });
            return documentCollection;
        }

        public static async Task<Database> GetDatabaseAsync(this DocumentClient client, string id)
        {
            var database = client.CreateDatabaseQuery().ToList().SingleOrDefault(xx => xx.Id == id);
            if (database == null)
                database = await client.CreateDatabaseAsync(new Database
                {
                    Id = id
                });
            return database;
        }
        public static async Task TryDeleteUDF(this DocumentClient client, string colSelfLink, string udfId)
        {
            UserDefinedFunction udf = client.CreateUserDefinedFunctionQuery(colSelfLink).Where(u => u.Id == udfId).AsEnumerable().FirstOrDefault();
            if (udf != null)
            {
                await client.DeleteUserDefinedFunctionAsync(udf.SelfLink);
            }
        }
        public static async Task TryDeleteStoredProcedure(this DocumentClient client, string colSelfLink, string sprocId)
        {
            StoredProcedure sproc = client.CreateStoredProcedureQuery(colSelfLink).Where(s => s.Id == sprocId).AsEnumerable().FirstOrDefault();
            if (sproc != null)
            {
                await client.DeleteStoredProcedureAsync(sproc.SelfLink);
            }
        }
        public static async Task TryDeleteTrigger(this DocumentClient client, string colSelfLink, string triggerId)
        {
            Trigger trigger = client.CreateTriggerQuery(colSelfLink).Where(t => t.Id == triggerId).AsEnumerable().FirstOrDefault();
            if (trigger != null)
            {
                await client.DeleteTriggerAsync(trigger.SelfLink);
            }
        }
    }
}

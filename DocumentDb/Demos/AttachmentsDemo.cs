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
    public static class AttachmentsDemo
    {
        public static async Task RunAsync(string[] args)
        {
            var client = DocumentDbExtension.CreateClient();
            var database = await client.GetDatabaseAsync("IoTEvents");
            var temperatureSamplesInsert = await client.GetCollectionAsync(database, "temperatureSamplesInsert");

            Document document = await client.NewTemperatureSampleDocumentAsync(temperatureSamplesInsert.SelfLink, "Pordenone", Region.North, 45, 12, Guid.NewGuid(), SensorClass.A, 24);

            //This attachment could be any binary you want to attach. Like images, videos, word documents, pdfs etc. it doesn't matter
            using (FileStream fileStream = new FileStream(@".\Attachments\LoremIpsum.txt", FileMode.Open))
            {
                //Create the attachment
                await client.CreateAttachmentAsync(document.AttachmentsLink, fileStream, new MediaOptions { ContentType = "text/plain", Slug = "LoremIpsum.txt" });
            }

            //Query for document for attachment for attachments
            Attachment attachment = client.CreateAttachmentQuery(document.SelfLink).AsEnumerable().FirstOrDefault();

            //Use DocumentClient to read the Media content
            MediaResponse content = await client.ReadMediaAsync(attachment.MediaLink);

            byte[] bytes = new byte[content.ContentLength];
            await content.Media.ReadAsync(bytes, 0, (int)content.ContentLength);
            string result = Encoding.UTF8.GetString(bytes);

            Console.ReadLine();
        }
    }
}

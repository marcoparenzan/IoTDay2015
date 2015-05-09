using DocumentDb.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDb.Tools
{
    public static class TemperatureSampleExtension
    {
        public static async Task<ResourceResponse<Document>> NewTemperatureSampleDocumentAsync(this DocumentClient client, string link, Location location, Sensor sensor, double value)
        {
            ResourceResponse<Document> response = null;
            try
            {
                var sample = new TemperatureSample
                {
                    Calendar = new Calendar
                    {
                        DateTime = DateTime.Now
                        ,
                        CalendarStripe = CalendarStripe.Morning
                    }
                    ,
                    Degree = new Degree { Value = value, Type = DegreeType.C }
                    ,
                    Location = location
                    ,
                    Sensor = sensor
                };
                response = await client.CreateDocumentAsync(link, sample);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public static async Task<ResourceResponse<Document>> NewTemperatureSampleDocumentAsync(this DocumentClient client, string link, string locationName, Region locationRegion, double lat, double lon, Guid sensorId, SensorClass sensorClass, double value)
        {
            return await NewTemperatureSampleDocumentAsync(
                client
                , link
                , new Location
                {
                    Name = locationName
                    ,
                    Region = locationRegion
                    ,
                    Latitude = lat
                    ,
                    Longitude = lon
                }
                , new Sensor
                {
                    Id = sensorId
                    ,
                    Class = sensorClass
                }
                , value
            );
        }
    }
}

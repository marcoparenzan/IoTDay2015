using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDb.Model
{
    public class SensorInfo
    {
        public string Type = typeof(SensorInfo).Name;

        [JsonProperty("id")]
        public string Id { get; set; }

        public SensorClass SensorClass { get; set; }

        public decimal Threshold { get; set; }
    }
}

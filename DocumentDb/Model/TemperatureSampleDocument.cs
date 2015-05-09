using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDb.Model
{
    public class TemperatureSampleDocument: Document
    {
        public Sensor Sensor { get; set; }
        public Degree Degree { get; set; }
        public Location Location { get; set; }
        public Calendar Calendar { get; set; }
    }
}

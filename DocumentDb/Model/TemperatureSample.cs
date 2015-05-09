using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDb.Model
{
    public class TemperatureSample: IComparable<TemperatureSample>, IEquatable<TemperatureSample>
    {
        public string Type = typeof(TemperatureSample).Name;

        public Sensor Sensor { get; set; }
        public Degree Degree { get; set; }
        public Location Location { get; set; }
        public Calendar Calendar { get; set; }

        public string RegionName
        {
            get
            {
                return Location.Region.ToString();
            }
        }

        int IComparable<TemperatureSample>.CompareTo(TemperatureSample other)
        {
            var compare = 0;
            compare = ((IComparable<Sensor>)Sensor).CompareTo(other.Sensor);
            if (compare != 0) return compare;
            compare = ((IComparable<Degree>)Degree).CompareTo(other.Degree);
            if (compare != 0) return compare;
            compare = ((IComparable<Location>)Location).CompareTo(other.Location);
            if (compare != 0) return compare;
            compare = ((IComparable<Calendar>)Calendar).CompareTo(other.Calendar);
            if (compare != 0) return compare;
            return compare;
        }

        bool IEquatable<TemperatureSample>.Equals(TemperatureSample other)
        {
            return ((IComparable)this).CompareTo(other) == 0;
        }
    }
}

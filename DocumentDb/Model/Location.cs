using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentDb.Model
{
    public class Location: IComparable<Location>
    {
        public string Name { get; set; }
        public Region Region { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        int IComparable<Location>.CompareTo(Location other)
        {
            var compare = 0;
            compare = ((IComparable<string>)Name).CompareTo(other.Name);
            if (compare != 0) return compare;
            compare = (int)Region - (int)other.Region;
            if (compare != 0) return compare;
            compare = (int) (Longitude - other.Longitude);
            if (compare != 0) return compare;
            compare = (int)(Latitude - other.Latitude);
            return compare;
        }
    }
}

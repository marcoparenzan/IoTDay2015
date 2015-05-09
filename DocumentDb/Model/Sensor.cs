using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDb.Model
{
    public class Sensor : IComparable<Sensor>
    {
        public Guid Id { get; set; }
        public SensorClass Class { get; set; }

        int IComparable<Sensor>.CompareTo(Sensor other)
        {
            var compare = 0;
            compare = ((IComparable<Guid>)Id).CompareTo(other.Id);
            if (compare != 0) return compare;
            compare = (int) Class - (int) other.Class;
            if (compare != 0) return compare;
            return compare;
        }
    }
}

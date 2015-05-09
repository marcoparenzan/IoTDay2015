using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDb.Model
{
    public class Calendar: IComparable<Calendar>
    {
        public DateTime DateTime { get; set; }
        public CalendarStripe CalendarStripe { get; set; }

        int IComparable<Calendar>.CompareTo(Calendar other)
        {
            var compare = 0;
            compare = (this.DateTime-other.DateTime).Milliseconds;
            if (compare != 0) return compare;
            compare = (int)CalendarStripe - (int)other.CalendarStripe;
            if (compare != 0) return compare;
            return compare;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTM.StartConsole
{
    static class Util
    {
        public static long DayTick(DateTime date)
        {
            DateTime ret = new DateTime(date.Year, date.Month, date.Day);
            return ret.Ticks;
        }
    }
}

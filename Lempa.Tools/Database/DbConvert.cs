using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lempa.Tools.Database
{
    public class DbConvert
    {
        public int DbToint(object value)
        {

            if (value == null) return -1;
            else
                return Convert.ToInt16(value);
        }
        public uint DbToUint(object value)
        {
            //TODO z001
            if (value == null) return 0;
            else
                return Convert.ToUInt16(value);
        }
        public ulong DbToUlong(object value)
        {

            if (value == null) return 0;
            else if (value is DBNull) return 0;
            else
                return Convert.ToUInt64(value);
        }
        public string DbToString(object value)
        {
            if (value == null) return "";
            else
                return value.ToString();
        }

        public DateTime DbToDateTime(object value)
        {
            if (value == null) return Convert.ToDateTime("2000-01-01 00:00:00");
            else
                return Convert.ToDateTime(value);
        }
        public bool DbToBool(object value)
        {
            if (value == null) return false;
            else
                return Convert.ToBoolean(value);
        }
        public string BoolToDb(bool value)
        {
            if (value)
                return "1";

            return "0";
        }
    }
}

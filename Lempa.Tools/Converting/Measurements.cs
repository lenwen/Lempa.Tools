using System;
using System.Collections.Generic;
using System.Text;

namespace Lempa.Tools.Converting
{
    public static class Measurements
    {
        public static double MeterPerSecond_Knots(double ms)
        {
            return (ms * 1.94384449);
        }
        public static double Meter_to_Mile(double meter)
        {
            return (meter * 0.000621371192);
        }
        public static double Mile_to_Meter(double mile)
        {
            return (mile * 1609.344);
        }
        public static double Feet_to_Km(double feet)
        {
            return (feet * 0.0003048);
        }
        public static double Feet_to_M(double feet)
        {
            return (feet * 0.3048);
        }
        public static Int32 Double_Int32_MathToEven(double value)
        {
            return Convert.ToInt32(Math.Round(value, MidpointRounding.ToEven));
        }

        public static ulong To_Ulong(string value)
        {
            double tmpValue = Convert.ToDouble(value.Replace(".", ","));
            return Convert.ToUInt64(Math.Round(tmpValue, MidpointRounding.ToEven));
        }


        public static float String_To_Float(string value)
        {
            return float.Parse(value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        /*
         * 
         * Use Math.Ceiling to round up

Math.Ceiling(0.5); // 1
Use Math.Round to just round

Math.Round(0.5, MidpointRounding.AwayFromZero); // 1
And Math.Floor to round down

Math.Floor(0.5); // 0
         * */
    }
}

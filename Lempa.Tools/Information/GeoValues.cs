using System;
using System.Collections.Generic;
using System.Text;

namespace Lempa.Tools.Information
{
    public static class GeoValues
    {
        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
        public static double RadiansToDegrees(double radians)
        {
            const double radToDegFactor = 180 / Math.PI;
            return radians * radToDegFactor;
        }

        /// <summary>
        /// https://en.wikipedia.org/wiki/Earth_radius
        /// </summary>
        public static double EarthRadiusKm { get { return 6371.230; } }
        /// <summary>
        /// https://en.wikipedia.org/wiki/Earth_radius
        /// </summary>
        public static double EarthEquatorialRadiusKm { get { return 6378.1370; } }
        /// <summary>
        /// https://en.wikipedia.org/wiki/Earth_radius
        /// </summary>
        public static double EarthPolarRadiusKm { get { return 6356.7523; } }
    }
}

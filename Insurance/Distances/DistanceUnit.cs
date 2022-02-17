namespace Distances {

    public struct DistanceUnit {

        internal readonly double MeterDistance;
        internal readonly string Symbol;
        internal readonly bool IsMetric;

        public static readonly DistanceUnit Millimeters = new DistanceUnit (0.001, "mm", true);
        public static readonly DistanceUnit Meters = new DistanceUnit (1, "m", true);
        public static readonly DistanceUnit Kilometers = new DistanceUnit (1000, "km", true);
        public static readonly DistanceUnit Inches = new DistanceUnit (0.0254, "in", false);
        public static readonly DistanceUnit Feet = new DistanceUnit (0.3048, "ft", false);
        public static readonly DistanceUnit Yards = new DistanceUnit (0.9144, "yd", false);
        public static readonly DistanceUnit Miles = new DistanceUnit (1609.344, "mi", false);

        private DistanceUnit (double meters, string symbol, bool isMetric) {
            MeterDistance = meters;
            Symbol = symbol;
            IsMetric = isMetric;
        }

    }

}

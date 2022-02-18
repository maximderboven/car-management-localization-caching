using System;

namespace Distances {

    public class Distance : IDistance {

        public double Value { get; }
        public DistanceUnit DistanceUnit { get; }

        public Distance (double distance, DistanceUnit distanceUnit) {
            Value = distance;
            DistanceUnit = distanceUnit;
        }

        public IDistance Convert (DistanceUnit unit) {
            return new Distance (Value * DistanceUnit.MeterDistance / unit.MeterDistance, unit);
        }

        public new string ToString () {
            return $"{Value} {DistanceUnit.Symbol}";
        }

        public string ToString (int roundingDigits) {
            if (roundingDigits < 0)
                throw new ArgumentOutOfRangeException (nameof (roundingDigits), "Value must be higher or equal to 0.");
            return $"{Math.Round (Value, roundingDigits)} {DistanceUnit.Symbol}";
        }
    }
}
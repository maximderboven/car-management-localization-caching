namespace Distances {

    public class Distance : IDistance {

        private readonly double _distance;
        private readonly DistanceUnit _distanceUnit;

        public Distance (double distance, DistanceUnit distanceUnit) {
            _distance = distance;
            _distanceUnit = distanceUnit;
        }

        IDistance IDistance.Convert (DistanceUnit unit) {
            return new Distance (_distance * _distanceUnit.MeterDistance / unit.MeterDistance, unit);
        }

        public new string ToString () {
            return $"{_distance} {_distanceUnit.Symbol}";
        }

        public string ToString (int roundingDigits) {
            if (roundingDigits < 0)
                throw new ArgumentOutOfRangeException (nameof (roundingDigits), "Value must be higher or equal to 0.");
            return $"{Math.Round (_distance, roundingDigits)} {_distanceUnit.Symbol}";
        }

    }

}
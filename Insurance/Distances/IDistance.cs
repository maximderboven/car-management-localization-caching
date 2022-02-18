namespace Distances {

    public interface IDistance {

        public double Value { get; }

        public DistanceUnit DistanceUnit { get; }

        public IDistance Convert (DistanceUnit unit);

        public string? ToString ();

        public string? ToString (int roundingDigits);

    }

}
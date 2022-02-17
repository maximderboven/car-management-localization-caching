namespace Distances {

    public interface IDistance {

        internal IDistance Convert (DistanceUnit unit);

        public string? ToString ();

        public string? ToString (int roundingDigits);

    }

}
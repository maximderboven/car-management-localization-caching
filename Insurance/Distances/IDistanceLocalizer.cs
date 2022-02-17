namespace Distances {

    public interface IDistanceLocalizer {

        IDistance Convert (IDistance distance, DistanceUnit metricUnit, DistanceUnit imperialUnit);

        IDistance Convert (double value, DistanceUnit originalUnit, DistanceUnit alternativeUnit);

    }

}
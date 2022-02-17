using System.Globalization;

namespace Distances;

public class DistanceLocalizer : IDistanceLocalizer {

    public IDistance Convert (IDistance distance, DistanceUnit metricUnit, DistanceUnit imperialUnit) {
        RegionInfo regionInfo = new RegionInfo (Thread.CurrentThread.CurrentCulture.Name);
        return regionInfo.IsMetric
            ? distance.Convert (metricUnit)
            : distance.Convert (imperialUnit);
    }

    public IDistance Convert (double value, DistanceUnit originalUnit, DistanceUnit alternativeUnit) {
        if (originalUnit.IsMetric == alternativeUnit.IsMetric)
            throw new ArgumentException ($"{nameof (originalUnit)} and {nameof (alternativeUnit)} can't both be imperial or both metric.");
        return originalUnit.IsMetric
            ? Convert (new Distance (value, originalUnit), originalUnit, alternativeUnit)
            : Convert (new Distance (value, alternativeUnit), alternativeUnit, originalUnit);
    }

}
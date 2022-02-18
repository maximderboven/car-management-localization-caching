using System;
using System.Globalization;
using System.Threading;

namespace Distances {

    public class DistanceLocalizer : IDistanceLocalizer {

        private RegionInfo GetRegionInfo () {
            return new RegionInfo (Thread.CurrentThread.CurrentCulture.Name);
        }

        public IDistance Localize (IDistance distance, DistanceUnit metricUnit, DistanceUnit imperialUnit) {
            return GetRegionInfo ().IsMetric
                ? distance.Convert (metricUnit)
                : distance.Convert (imperialUnit);
        }

        public IDistance Localize (double value, DistanceUnit originalUnit, DistanceUnit alternativeUnit) {
            if (originalUnit.IsMetric == alternativeUnit.IsMetric)
                throw new ArgumentException ($"{nameof (originalUnit)} and {nameof (alternativeUnit)} can't both be imperial or both metric.");
            return originalUnit.IsMetric
                ? Localize (new Distance (value, originalUnit), originalUnit, alternativeUnit)
                : Localize (new Distance (value, alternativeUnit), alternativeUnit, originalUnit);
        }

        public double Delocalize (double value, DistanceUnit wantedUnit, DistanceUnit alternativeUnit) {
            if (wantedUnit.IsMetric == alternativeUnit.IsMetric)
                throw new ArgumentException ($"{nameof (wantedUnit)} and {nameof (alternativeUnit)} can't both be imperial or both metric.");
            if (GetRegionInfo ().IsMetric == wantedUnit.IsMetric)
                return value;

            return new Distance (value, alternativeUnit).Convert (wantedUnit).Value;

        }

        public string GetSymbol (DistanceUnit metricUnit, DistanceUnit imperialMetric) {
            return GetRegionInfo ().IsMetric
                ? metricUnit.Symbol
                : imperialMetric.Symbol;
        }

    }

}
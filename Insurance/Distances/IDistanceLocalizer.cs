namespace Distances {

    public interface IDistanceLocalizer {

        IDistance Localize (IDistance distance, DistanceUnit metricUnit, DistanceUnit imperialUnit);

        IDistance Localize (double value, DistanceUnit originalUnit, DistanceUnit alternativeUnit);

        /// <summary>
        /// Converts inputs that the user puts in back to values the system expects.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="wantedUnit">The unit that the rest of the program expects.</param>
        /// <param name="alternativeUnit">The alternative unit that can be converted from if the cultures don't match.</param>
        /// <returns></returns>
        double Delocalize (double value, DistanceUnit wantedUnit, DistanceUnit alternativeUnit);

        string GetSymbol (DistanceUnit metricUnit, DistanceUnit imperialMetric);

    }

}
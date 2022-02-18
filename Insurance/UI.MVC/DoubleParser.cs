using System.Globalization;

namespace UI.MVC {

    public class DoubleParser : IDoubleParser {

        public double Parse (string value) {
            return double.Parse (value.Replace (',','.'), NumberStyles.Any, CultureInfo.InvariantCulture);
        }

    }

}
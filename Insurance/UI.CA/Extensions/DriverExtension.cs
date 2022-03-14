using System.Linq;
using System.Text;
using Insurance.Domain;
using Resources;

namespace Insurance.UI.CA.Extensions
{
    internal static class DriverExtension
    {
        internal static string GetInfo(this Driver d, bool showCars = false)
        {
            var sb = new StringBuilder($"{d.LastName} {d.FirstName} ({string.Format (ViewLocalizationResources.Born_On_Date, d.DateOfBirth.ToShortDateString ())})\n");
            if (!showCars || !d.Rentals.Any()) return sb.ToString();
            foreach (var c in d.Rentals)
            {
                sb.AppendLine($"\t{PropertyResources.Car}: {PropertyResources.NumberPlate}: {c.Car.NumberPlate} {ViewLocalizationResources.From} {c.Car.Brand}");
            }
            return sb.ToString();
        }
    }
}
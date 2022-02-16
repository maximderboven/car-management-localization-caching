using System;
using System.Linq;
using System.Text;
using Insurance.Domain;

namespace Insurance.UI.CA.Extensions
{
    internal static class DriverExtension
    {
        internal static string GetInfo(this Driver d, bool showCars = false)
        {
            var sb = new StringBuilder($"{d.LastName} {d.FirstName} (born on {d.DateOfBirth.ToShortDateString()})\n");
            if (!showCars || !d.Rentals.Any()) return sb.ToString();
            foreach (var c in d.Rentals)
            {
                sb.AppendLine($"\tCar: numberplate: {c.Car.NumberPlate} from {c.Car.Brand}");
            }
            return sb.ToString();
        }
    }
}
using System;
using Insurance.Domain;

namespace Insurance.UI.CA.Extensions
{
    internal static class CarExtension
    {
        internal static string GetInfo(this Car c)
        {
            return c.Garage != null ? $"Car with numberplate: {c.NumberPlate} from {c.Brand} - on {c.Fuel.ToString()} [Managed by garage '{c.Garage.Name}'] {c.PurchasePrice}" : $"Car with numberplate: {c.NumberPlate} from {c.Brand} - on {c.Fuel.ToString()} {c.PurchasePrice}";
        }

    }
}
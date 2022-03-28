using Insurance.Domain;
using Resources;

namespace Insurance.UI.CA.Extensions
{
    internal static class CarExtension
    {
        internal static string GetInfo(this Car c)
        {
            string garage = c.Garage != null
                ? string.Format(ViewLocalizationResources.Managed_By_Garage, c.Garage.Name)
                : string.Empty;
            return string.Format (ViewLocalizationResources.GetInfo_Car,
                c.NumberPlate, c.Brand, c.Fuel.GetName (), garage, c.PurchasePrice);
        }

    }
}

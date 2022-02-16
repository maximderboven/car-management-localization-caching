using Insurance.Domain;

namespace Insurance.UI.CA.Extensions
{
    internal static class GarageExtension
    {
        internal static string GetInfo(this Garage g)
        {
            return $"Garage {g.Name}";
        }
    }
}
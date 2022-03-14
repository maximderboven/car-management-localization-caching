using Insurance.Domain;
using Resources;

namespace Insurance.UI.CA.Extensions
{
    internal static class GarageExtension
    {
        internal static string GetInfo(this Garage g)
        {
            return $"{PropertyResources.Garage} {g.Name}";
        }
    }
}
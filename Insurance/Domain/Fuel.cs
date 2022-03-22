using Resources;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain
{
    public enum Fuel : byte
    {
        [Display (ResourceType = typeof (PropertyResources), Name = "Gas")]
        Gas,
        [Display (ResourceType = typeof (PropertyResources), Name = "Oil")]
        Oil,
        [Display (ResourceType = typeof(PropertyResources), Name = "Lpg")]
        Lpg

    }

    public static class FuelExtensions {

        public static string GetName (this Fuel fuel) {
            return PropertyResources.ResourceManager.GetString ($"Fuel_{fuel}");
        }

    }
}
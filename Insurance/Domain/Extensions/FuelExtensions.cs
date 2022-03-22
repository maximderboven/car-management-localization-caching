using Resources;

namespace Insurance.Domain.Extensions {

    public static class FuelExtensions {

        public static string GetName (this Fuel fuel) {
            return PropertyResources.ResourceManager.GetString ($"Fuel_{fuel}");
        }

    }

}
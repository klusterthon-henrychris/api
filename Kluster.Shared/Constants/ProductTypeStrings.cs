namespace Kluster.Shared.Constants
{
    public static class ProductTypeStrings
    {
        public const string Physical = "Physical";
        public const string Digital = "Digital";

        public static string[] AllProductTypeOptions = [Physical, Digital];
    }

    public enum ProductType
    {
        Physical,
        Digital
    }
}
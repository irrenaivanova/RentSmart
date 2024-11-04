namespace RentSmart.Common
{
    public static class EntityValidationConstants
    {
        public static class Property
        {
            public const int MinLengthName = 5;
            public const int MaxLengthName = 50;

            public const int MinLengthDescription = 50;
            public const int MaxLengthDescription = 1000;

            public const string PricePerMonthMinValue = "0";
            public const string PricePerMonthMaxValue = "5000";
        }

        public static class PropertyType
        {
            public const int MaxLengthName = 50;
        }

        public static class District
        {
            public const int MaxLengthName = 30;
        }

        public static class City
        {
            public const int MaxLengthName = 30;
        }

        public static class Tag
        {
            public const int MaxLengthName = 50;
        }

        public static class Image
        {
            public const int UrlMaxLength = 2048;
            public const int ExtensionMaxLength = 256;
        }

        public static class Manager
        {
            public const int UrlMaxLength = 2048;
        }
    }
}

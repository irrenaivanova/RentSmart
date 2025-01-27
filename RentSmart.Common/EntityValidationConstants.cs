﻿namespace RentSmart.Common
{
    public static class EntityValidationConstants
    {
        public const int UrlMaxLength = 2048;
        public const int ExtensionMaxLength = 256;

        public static class Property
        {
            public const int MinLengthDescription = 50;
            public const int MaxLengthDescription = 1000;

            public const string PricePerMonthMinValue = "100";
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

        public static class Rating
        {
            public const int MinRate = 1;
            public const int MaxRate = 5;
        }

        public static class Service
        {
            public const int MaxLengthName = 50;
            public const int MaxLengthDescription = 400;
        }

        public static class ApplicationUser
        {
            public const int MinLengthName = 2;
            public const int MaxLengthName = 50;
        }

        public static class Rental
        {
            public const int MinRentalDuration = 1;
            public const int MaxRentalDuration = 24;
        }
    }
}

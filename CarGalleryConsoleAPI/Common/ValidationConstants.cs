namespace CarGalleryConsoleAPI.Common
{
    public class ValidationConstants
    {
        public const int MakeMaxLength = 60;

        public const int ModelMaxLength = 80;

        public const string CatalogNumberFormat = @"^([0-9A-Z]{12})$";

        public const int YearMinValue = 1886;
        public const int YearMaxValue = 2024;

        public const int MileageMinValue = 0;
        public const int MileageMaxValue = 350_000;
    }
}

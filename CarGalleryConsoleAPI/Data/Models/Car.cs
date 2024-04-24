using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarGalleryConsoleAPI.Common;

namespace CarGalleryConsoleAPI.Data.Model
{
    public class Car
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.CatalogNumberFormat)]
        public string CatalogNumber { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MakeMaxLength)]
        public string Make { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ModelMaxLength)]
        public string Model { get; set; }

        public string? Color { get; set; }

        [Required]
        [Range(ValidationConstants.YearMinValue, ValidationConstants.YearMaxValue)]
        public int Year { get; set; }

        [Required]
        [Range(ValidationConstants.MileageMinValue, ValidationConstants.MileageMaxValue)]
        public int Mileage { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public bool Availability { get; set; }
    }
}



using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ToyStore.API.Models
{
    public class Product
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public required string Name { get; set; }

        [StringLength(100, ErrorMessage = "Maximum 100 characters")]
        public string? Description { get; set; }

        [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
        public int? AgeRestriction { get; set; }

        [Required(ErrorMessage = "Company is required")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public required string Company { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 1000, ErrorMessage = "Must be between 1 and 1000")]
        public decimal Price { get; set; }
    }
}

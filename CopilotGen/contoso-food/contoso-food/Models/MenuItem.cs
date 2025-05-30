using System.ComponentModel.DataAnnotations;

namespace contoso_food.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Range(0, 100)]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; } = string.Empty; // e.g., Breakfast, Lunch
    }
}

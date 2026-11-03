using System.ComponentModel.DataAnnotations;

namespace UserManager.Data.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        [Required]
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}


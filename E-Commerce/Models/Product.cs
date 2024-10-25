using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, 1000000)]
        public decimal Price { get; set; }
        [Range(0, 5)]
        public int Rating { get; set; }
        [ValidateNever]
        public string PhotoUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public int? CompanyId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public Company Company { get; set; }
    }
}

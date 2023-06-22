using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Exercise3.Models.DTOs
{
    public class AnimalPOST
    {
        [Required]
        [BindRequired]
        public int ID { get; set; }
        [Required]
        [MaxLength(200)]
        [BindRequired]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        [MinLength(1)]
        public string Description { get; set; } = null;
        [Required]
        [BindRequired]
        [MaxLength(200)]
        [MinLength(1)]
        public string Category { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        [BindRequired]
        [MinLength(1)]
        public string Area { get; set; } = string.Empty;
    }
}

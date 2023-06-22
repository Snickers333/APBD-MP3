using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Exercise3.Models.DTOs
{
    public class AnimalPUT
    {
        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        [BindRequired]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        [MinLength(1)]
        public string Description { get; set; } = null;
        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        [BindRequired]
        public string Category { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        [BindRequired]
        public string Area { get; set; } = string.Empty;
    }
}

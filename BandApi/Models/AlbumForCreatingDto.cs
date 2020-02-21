using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BandApi.Models
{
    public class AlbumForCreatingDto : IValidatableObject
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == Description)
            {
                yield return new ValidationResult("Title Must Be Different From Description", new[] { "AlbumForCreatingDto" });
            }
        }
    }
}

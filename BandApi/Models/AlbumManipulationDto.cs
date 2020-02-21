using System;
using System.ComponentModel.DataAnnotations;
using BandApi.ValidationAttributes;

namespace BandApi.Models
{
    [TitleAndDescription(ErrorMessage = "Title must be different from description")]
    public abstract class AlbumManipulationDto
    {
        [Required]
        [MaxLength(200, ErrorMessage = "Title has max of 200 characters")]
        public string Title { get; set; }

        [MaxLength(400)]
        public virtual string Description { get; set; }

    }
}

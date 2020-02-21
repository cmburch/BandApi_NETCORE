using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BandApi.ValidationAttributes;

namespace BandApi.Models
{
    [TitleAndDescription]
    public class AlbumForCreatingDto 
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

    }
}

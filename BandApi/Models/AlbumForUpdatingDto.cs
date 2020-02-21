using System;
using System.ComponentModel.DataAnnotations;
using BandApi.ValidationAttributes;

namespace BandApi.Models
{
    public class AlbumForUpdatingDto : AlbumManipulationDto
    {
        [Required(ErrorMessage = "You need to fill description")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
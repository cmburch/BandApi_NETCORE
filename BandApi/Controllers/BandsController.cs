using System;
using System.Collections.Generic;
using BandApi.Models;
using BandApi.Services;
using BandApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BandApi.Controllers
{
    [ApiController]
    [Route("api/bands")]
    public class BandsController : ControllerBase
    {
        private readonly IBandAlbumRepository _bandAlbumRepository;

        public BandsController(IBandAlbumRepository bandAlbumRepository)
        {
            _bandAlbumRepository = bandAlbumRepository ??
                throw new ArgumentNullException(nameof(bandAlbumRepository));
        }

        [HttpGet]
        public ActionResult<IEnumerable<BandDto>> GetBands()
        {
            var bandsFromRepo = _bandAlbumRepository.GetBands();
            var bandsDto = new List<BandDto>();

            foreach (var band in bandsFromRepo)
            {
                bandsDto.Add(new BandDto()
                {
                    Id = band.Id,
                    Name = band.Name,
                    MainGenre = band.MainGenre,
                    FoundedYearsAgo = $"{ band.Founded.ToString("yyyy") } ({ band.Founded.GetYearsAgo() } years ago)"

                });
            }
            return Ok(bandsDto);
        }

        [HttpGet("{bandId}")]
        public IActionResult GetBand(Guid bandId)
        {
            var bandFromRepo = _bandAlbumRepository.GetBand(bandId);

            if (bandFromRepo == null)
                return NotFound();

            return Ok(bandFromRepo);
        }
    }
}

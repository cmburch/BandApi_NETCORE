using System;
using System.Collections.Generic;
using BandApi.Models;
using BandApi.Services;
using BandApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace BandApi.Controllers
{
    [ApiController]
    [Route("api/bands")]
    public class BandsController : ControllerBase
    {
        private readonly IBandAlbumRepository _bandAlbumRepository;
        private readonly IMapper _mapper;

        public BandsController(IBandAlbumRepository bandAlbumRepository, IMapper mapper)
        {
            _bandAlbumRepository = bandAlbumRepository ??
                throw new ArgumentNullException(nameof(bandAlbumRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{albumId}",Name = "GetAlbumForBand")]
        public ActionResult<IEnumerable<BandDto>> GetBands([FromQuery]BandsResourceParameters bandsResourceParameters)
        {
            var bandsFromRepo = _bandAlbumRepository.GetBands(bandsResourceParameters);

            return Ok(_mapper.Map<IEnumerable<BandDto>>(bandsFromRepo));
        }

        [HttpGet("{bandId}", Name = "GetBand")]
        public IActionResult GetBand(Guid bandId)
        {
            var bandFromRepo = _bandAlbumRepository.GetBand(bandId);

            if (bandFromRepo == null)
                return NotFound();

            return Ok(bandFromRepo);
        }

        [HttpPost]
        public ActionResult<BandDto> CreateBand([FromBody] BandForCreatingDto band)
        {
            var bandEntity = _mapper.Map<Entities.Band>(band); //map to a object the database understands
            _bandAlbumRepository.AddBand(bandEntity); //currently the id property is 00000 and once the entity is added to the context then it gets a GUID Id
            _bandAlbumRepository.Save();

            var bandToReturn = _mapper.Map<BandDto>(bandEntity);

            return CreatedAtRoute("GetBand", new { bandId = bandToReturn.Id }, bandToReturn); //Header Location →https://localhost:5001/api/bands/08d7b5bc-cb24-422d-821b-a3d10226036e
        }

    }
}

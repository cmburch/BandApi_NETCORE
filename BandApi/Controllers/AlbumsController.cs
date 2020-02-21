using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using BandApi.Models;
using BandApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BandApi.Controllers
{
    [ApiController]
    [Route("api/bands/{bandId}/albums")]
    public class AlbumsController : ControllerBase
    {
        private readonly IBandAlbumRepository _bandAlbumRepository;
        private readonly IMapper _mapper;

        public AlbumsController(IBandAlbumRepository bandAlbumRepository, IMapper mapper)
        {
            _bandAlbumRepository = bandAlbumRepository ??
                throw new ArgumentNullException(nameof(bandAlbumRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<AlbumsDto>> GetAlbumsForBand(Guid bandId)
        {
            if (!_bandAlbumRepository.BandExists(bandId))
                return NotFound();

            var albumsFromRepo = _bandAlbumRepository.GetAlbums(bandId);
            return Ok(_mapper.Map<IEnumerable<AlbumsDto>>(albumsFromRepo));
        }

        [HttpGet("{albumId}")]
        public ActionResult<AlbumsDto> GetAlbumForBand(Guid bandId, Guid albumId)
        {
            if (!_bandAlbumRepository.BandExists(bandId))
                return NotFound();

            var albumFromRepo = _bandAlbumRepository.GetAlbum(bandId, albumId);

            if (albumFromRepo == null)
                return NotFound();

            return Ok(_mapper.Map<AlbumsDto>(albumFromRepo));
        }


        [HttpPost]
        public ActionResult<AlbumsDto> CreateAlbumForBand(Guid bandId, [FromBody]AlbumForCreatingDto album)
        {
            if (!_bandAlbumRepository.BandExists(bandId))
                return NotFound();

            var albumEntity = _mapper.Map<Entities.Album>(album); //map to a object the database understands
            _bandAlbumRepository.AddAlbum(bandId, albumEntity);
            _bandAlbumRepository.Save();

            var albumToReturn = _mapper.Map<AlbumsDto>(albumEntity);

            return CreatedAtRoute("GetAlbumForBand",
                new { bandId = bandId, albumId = albumToReturn.Id }, albumToReturn); //return a location header and 201 created response
        }


        [HttpPut("{albumId}")]
        public ActionResult UpdateAlbumForBand(Guid bandId, Guid albumId,[FromBody] AlbumForUpdatingDto album)
        {
            if (!_bandAlbumRepository.BandExists(bandId))
                return NotFound();

            var albumFromRepo = _bandAlbumRepository.GetAlbum(bandId, albumId);
            if (albumFromRepo == null)
                return NotFound();

            //pass the album values to albumFromRepo this is the object in the DBcontext
            //albumFromRepo is updated and its in a modified state inside of the Dbcontext
            _mapper.Map(album, albumFromRepo); //the context is update here
            //_bandAlbumRepository.UpdateAlbum(albumFromRepo); //not implemented in repository
            _bandAlbumRepository.Save();

            return NoContent();
        }
        
    }
}

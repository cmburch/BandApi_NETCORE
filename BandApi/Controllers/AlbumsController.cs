using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using BandApi.Models;
using BandApi.Services;
using Microsoft.AspNetCore.JsonPatch;
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
            _mapper.Map(album, albumFromRepo); //the context is updated here, leave off type annotation on Map because put should update all of the properties
            //_bandAlbumRepository.UpdateAlbum(albumFromRepo); //not implemented in repository
            _bandAlbumRepository.Save();

            return NoContent();
        }

        [HttpPatch("{albumId}")]
        public ActionResult PartiallyUpdateAlbumForBand(Guid bandId, Guid albumId,[FromBody] JsonPatchDocument<AlbumForUpdatingDto> patchDocument)
        {
            if (!_bandAlbumRepository.BandExists(bandId))
                return NotFound();

            var albumFromRepo = _bandAlbumRepository.GetAlbum(bandId, albumId);
            if (albumFromRepo == null)
                return NotFound();

            var albumToPatch = _mapper.Map<AlbumForUpdatingDto>(albumFromRepo); // example : this has the { Title and Description } Property
            patchDocument.ApplyTo(albumToPatch); // this is where the albumToPatch is updated with the new values from patchDocument

            _mapper.Map(albumToPatch, albumFromRepo); //take source albumToPatch and map to destination albumFromRepo, context is modified
            //_bandAlbumRepository.UpdateAlbum(albumFromRepo); //this method is not implemented in repository
            _bandAlbumRepository.Save();

            return NoContent();
        }

    }
}

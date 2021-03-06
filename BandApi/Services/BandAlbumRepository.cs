﻿using System;
using System.Collections.Generic;
using System.Linq;
using BandApi.DbContexts;
using BandApi.Entities;
using BandApi.Helpers;

namespace BandApi.Services
{
    public class BandAlbumRepository : IBandAlbumRepository
    {
        private readonly BandAlbumContext _context;

        public BandAlbumRepository(BandAlbumContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public void AddAlbum(Guid bandId, Album album)
        {
            if (bandId == Guid.Empty)
                throw new ArgumentNullException(nameof(bandId));

            if (album == null)
                throw new ArgumentNullException(nameof(album));

            album.BandId = bandId; //associate the album to a band
            _context.Albums.Add(album); // Add the album to the context
        }

        public void AddBand(Band band)
        {
            if (band == null)
                throw new ArgumentNullException(nameof(band));

            _context.Bands.Add(band);
        }

        public bool AlbumExists(Guid albumId)
        {
            if (albumId == Guid.Empty)
                throw new ArgumentNullException(nameof(albumId));

            return _context.Albums.Any(a => a.Id == albumId);//compare the passed in albumId to Albums Id in the context
        }

        public bool BandExists(Guid bandId)
        {
            if(bandId == Guid.Empty)
                throw new ArgumentNullException(nameof(bandId));

            return _context.Bands.Any(b => b.Id == bandId);

        }

        public void DeleteAlbum(Album album)
        {
            if(album == null)
                throw new ArgumentNullException(nameof(album));

            _context.Albums.Remove(album);
        }

        public void DeleteBand(Band band)
        {
            if (band == null)
                throw new ArgumentNullException(nameof(band));

            _context.Bands.Remove(band);
        }

        public Album GetAlbum(Guid bandId, Guid albumId)
        {
            if (bandId == Guid.Empty)
                throw new ArgumentNullException(nameof(bandId));

            if (albumId == Guid.Empty)
                throw new ArgumentNullException(nameof(albumId));

            return _context.Albums.Where(a => a.BandId == bandId && a.Id == albumId).FirstOrDefault();
        }

        public IEnumerable<Album> GetAlbums(Guid bandId)
        {
            if (bandId == Guid.Empty)
                throw new ArgumentNullException(nameof(bandId));

            return _context.Albums.Where(a => a.BandId == bandId)
                .OrderBy(a => a.Title).ToList();
        }

        public Band GetBand(Guid bandId)
        {
            if (bandId == Guid.Empty)
                throw new ArgumentNullException(nameof(bandId));

            return _context.Bands.FirstOrDefault(b => b.Id == bandId);
        }

        public IEnumerable<Band> GetBands()
        {
            return _context.Bands.ToList();
        }

        public IEnumerable<Band> GetBands(IEnumerable<Guid> bandIds)
        {
            if (bandIds == null)
                throw new ArgumentNullException(nameof(bandIds));

            return _context.Bands.Where(b => bandIds.Contains(b.Id))
                .OrderBy(b => b.Name).ToList();
        }

        public IEnumerable<Band> GetBands(BandsResourceParameters bandsResourceParameters)
        {
            if (bandsResourceParameters == null)
                throw new ArgumentNullException(nameof(bandsResourceParameters));

            if (string.IsNullOrWhiteSpace(bandsResourceParameters.MainGenre) && string.IsNullOrWhiteSpace(bandsResourceParameters.SearchQuery))
                return GetBands(); //get all the bands if the genre and search is null or whitespace

            var collection = _context.Bands as IQueryable<Band>;

            if (!string.IsNullOrWhiteSpace(bandsResourceParameters.MainGenre))
            {
                var mainGenre = bandsResourceParameters.MainGenre.Trim();
                collection = collection.Where(b => b.MainGenre == mainGenre);
            }


            if (!string.IsNullOrWhiteSpace(bandsResourceParameters.SearchQuery))
            {
                var searchQuery = bandsResourceParameters.SearchQuery.Trim();
                collection = collection.Where(b => b.Name.Contains(searchQuery));
            }

            return collection.ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0); // SaveChanges return an integer of save records and -1 if save failed
        }

        public void UpdateAlbum(Album album)
        {
            throw new NotImplementedException();
        }

        public void UpdateBand(Band band)
        {
            throw new NotImplementedException();
        }
    }
}

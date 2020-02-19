using System;
using System.Collections.Generic;
using BandApi.Entities;

namespace BandApi.Services
{
    public interface IBandAlbumRepository
    {
        //ALBUMS
        IEnumerable<Album> GetAlbums(Guid bandId);

        Album GetAlbum(Guid bandId, Guid albumId);

        void AddAlbum(Guid bandId, Album album);

        void UpdateAlbum(Album album);

        void DeleteAlbum(Album album);


        //BANDS
        IEnumerable<Band> GetBands();

        Band GetBand(Guid bandId);

        IEnumerable<Band> GetBands(IEnumerable<Guid> bandIds);

        IEnumerable<Band> GetBands(string mainGenre);

        void AddBand(Band band);

        void UpdateBand(Band band);

        void DeleteBand(Band band);



        bool BandExists(Guid bandId);

        bool AlbumExists(Guid albumId);

        bool Save();


    }
}

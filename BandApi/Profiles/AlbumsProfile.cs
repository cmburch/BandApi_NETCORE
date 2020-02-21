using System;
using AutoMapper;
using BandApi.Models;

namespace BandApi.Profiles
{
    public class AlbumsProfile : Profile
    {
        public AlbumsProfile()
        {
            CreateMap<Entities.Album, Models.AlbumsDto>().ReverseMap();

            CreateMap<AlbumForCreatingDto, Entities.Album>();

            CreateMap<Models.AlbumForUpdatingDto, Entities.Album>();

        }
    }
}


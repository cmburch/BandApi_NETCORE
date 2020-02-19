using System;
using AutoMapper;
using BandApi.Helpers;

namespace BandApi.Profiles
{
    public class BandsProfile : Profile
    {
        public BandsProfile()
        {
            CreateMap<Entities.Band, Models.BandDto>()
                .ForMember(
                    dest => dest.FoundedYearsAgo,
                    opt => opt.MapFrom(src => $"{src.Founded.ToString("yyyy")} ({src.Founded.GetYearsAgo()}) years ago"));

            CreateMap<Models.BandForCreatingDto, Entities.Band>();//takes a BandForCreatingDto and maps it to the Band entity
        }
    }
}

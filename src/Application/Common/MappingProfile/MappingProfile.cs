using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UrlShortenerService.Application.Url.Commands.Response;

namespace UrlShortenerService.Application.Common.MappingProfile;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        _ = CreateMap<UrlShortenerService.Domain.Entities.Url, UrlDetailResponse>();
    }
}

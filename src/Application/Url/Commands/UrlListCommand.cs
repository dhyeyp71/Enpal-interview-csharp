using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HashidsNet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortenerService.Application.Common.Exceptions;
using UrlShortenerService.Application.Common.Interfaces;
using UrlShortenerService.Application.Url.Commands.Response;
using UrlShortenerService.Domain.Entities;

namespace UrlShortenerService.Application.Url.Commands;
public record UrlListCommand : IRequest<List<UrlDetailResponse>>
{
}

public class UrlListCommandHandler : IRequestHandler<UrlListCommand, List<UrlDetailResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IHashids _hashids;

    public UrlListCommandHandler(IApplicationDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }
    public async Task<List<UrlDetailResponse>> Handle(UrlListCommand request, CancellationToken cancellationToken)
    {
        var urlList = await _context.Urls.ToListAsync();
        List<UrlDetailResponse> list = new List<UrlDetailResponse>();

        if (urlList != null && urlList.Count > 0)
        {
            foreach (var uList in urlList)
            {
                list.Add(new UrlDetailResponse() { Id = _hashids.EncodeLong(uList.Id), OriginalUrl = uList.OriginalUrl });
            }
        }

        return list;
    }
}

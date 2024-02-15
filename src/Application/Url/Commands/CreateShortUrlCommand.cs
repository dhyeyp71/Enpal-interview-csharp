using FluentValidation;
using HashidsNet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortenerService.Application.Common.Interfaces;
using UrlShortenerService.Domain.Entities;

namespace UrlShortenerService.Application.Url.Commands;

public record CreateShortUrlCommand : IRequest<string>
{
    public string Url { get; init; } = default!;
}

public class CreateShortUrlCommandValidator : AbstractValidator<CreateShortUrlCommand>
{
    public CreateShortUrlCommandValidator()
    {
        _ = RuleFor(v => v.Url)
          .NotEmpty()
          .WithMessage("Url is required.");
    }
}

public class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IHashids _hashids;

    public CreateShortUrlCommandHandler(IApplicationDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    public async Task<string> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        //Check weather url is exist
        var existingUrl = await _context.Urls.FirstOrDefaultAsync(u => u.OriginalUrl == request.Url);

        if (existingUrl != null)
        {
            return _hashids.EncodeLong(existingUrl.Id);
        }

        var newUrl = new UrlShortenerService.Domain.Entities.Url()
        {
            OriginalUrl = request.Url,
            Created = DateTime.Now
        };

        _context.Urls.Add(newUrl);

        await _context.SaveChangesAsync(cancellationToken);

        return _hashids.EncodeLong(newUrl.Id);
        //return await  Task.FromResult(_hashids.EncodeLong(newUrl.Id));

        //await Task.CompletedTask;
        //throw new NotImplementedException();
    }
}

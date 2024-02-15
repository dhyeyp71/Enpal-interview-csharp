using FluentValidation;
using HashidsNet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortenerService.Application.Common.Exceptions;
using UrlShortenerService.Application.Common.Interfaces;

namespace UrlShortenerService.Application.Url.Commands;

public record RedirectToUrlCommand : IRequest<string>
{
    public string Id { get; init; } = default!;
}

public class RedirectToUrlCommandValidator : AbstractValidator<RedirectToUrlCommand>
{
    private readonly IHashids _hashids;
    public RedirectToUrlCommandValidator(IHashids hashids)
    {
        _hashids = hashids;

        _ = RuleFor(v => v.Id)
          .NotEmpty()
          .WithMessage("Id is required.")
          .Must(ValidId)
          .WithMessage("Invalid Short Url");
    }

    private bool ValidId(string id)
    {
        return _hashids.TryDecodeSingleLong(id, out _);
    }
}

public class RedirectToUrlCommandHandler : IRequestHandler<RedirectToUrlCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IHashids _hashids;

    public RedirectToUrlCommandHandler(IApplicationDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    public async Task<string> Handle(RedirectToUrlCommand request, CancellationToken cancellationToken)
    {
        var existingUrl = await _context.Urls.FirstOrDefaultAsync(u => u.Id == _hashids.DecodeSingleLong(request.Id));

        if (existingUrl != null)
        {
            return existingUrl.OriginalUrl;
        }

        throw new NotFoundException("Url not found");


        //await Task.CompletedTask;
        //throw new NotImplementedException();
    }
}

using MediatR;
using UrlShortenerService.Api.Endpoints;
using UrlShortenerService.Api.Endpoints.Url;
using UrlShortenerService.Application.Url.Commands;
using UrlShortenerService.Application.Url.Commands.Response;
using IMapper = AutoMapper.IMapper;

namespace Api.Endpoints.Url;


public class UrlListSummary : Summary<UrlListEndpoint>
{
    public UrlListSummary()
    {
        Summary = "Get list of short url and original url";
        Description =
            "This endpoint will return list of short url.";
        Response(500, "Internal server error.");
    }
}

public class UrlListEndpoint : BaseEndpoint<EmptyRequest, List<UrlDetailResponse>>
{
    public UrlListEndpoint(ISender mediator, IMapper mapper)
        : base(mediator, mapper) { }

    public override void Configure()
    {
        base.Configure();
        Get("uList/");
        AllowAnonymous();
        Description(
            d => d.WithTags("Url")
        );
        Summary(new CreateShortUrlSummary());
    }

    public override async Task HandleAsync(EmptyRequest er, CancellationToken ct)
    {
        var result = await Mediator.Send(
                        new UrlListCommand(),
                        ct);

        await SendOkAsync(result);
    }

}

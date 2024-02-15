using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortenerService.Application.Url.Commands.Response;

public class UrlDetailResponse
{
    public string Id { get; set; }
    public string OriginalUrl { get; set; }
}

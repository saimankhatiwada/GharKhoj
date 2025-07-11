﻿using GharKhoj.Api.MimeTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace GharKhoj.Api.Models.Common;

public record AcceptHeaderDto
{
    [FromHeader(Name = "Accept")]
    public string? Accept { get; init; }

    public bool IncludeLinks =>
        MediaTypeHeaderValue.TryParse(Accept, out MediaTypeHeaderValue? mediaType) &&
        mediaType.SubTypeWithoutSuffix.HasValue &&
        mediaType.SubTypeWithoutSuffix.Value.Contains(CustomMimeTypeNames.Application.HateoasSubType);
}

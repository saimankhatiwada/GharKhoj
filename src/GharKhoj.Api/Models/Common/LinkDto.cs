﻿namespace GharKhoj.Api.Models.Common;

public sealed class LinkDto
{
    public required string Href { get; init; }
    public required string Rel { get; init; }
    public required string Method { get; init; }
}

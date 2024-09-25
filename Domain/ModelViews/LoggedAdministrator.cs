﻿namespace MinimalApi.Domain.ModelViews;

public record LoggedAdministrator
{
    public string Email { get; set; } = default!;
    public string Profile { get; set; } = default!;
    public string Token { get; set; } = default!;
}